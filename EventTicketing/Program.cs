using AutoMapper;
using EventTicketing.Data.Mappings;
using EventTicketing.Data.Mappings.Events;
using EventTicketing.Data.Repositories.Events;
using EventTicketing.Data.Repositories.Tickets;
using EventTicketing.Data.Repositories.TicketSalesRepository;
using EventTicketing.Endpoints.Event;
using EventTicketing.Endpoints.Ticket;
using EventTicketing.Services.Events;
using EventTicketing.Services.Tickets;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.OpenApi.Models;
using NHibernate;
using NHibernate.Dialect;
using NHibernate.Driver;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "EventTicketing_";
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevelopmentCors", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "https://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});



// Configuration
builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Event Ticketing API", Version = "v1" });
});

// NHibernate Configuration for SQLite
builder.Services.AddSingleton<ISessionFactory>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var connectionString = config.GetConnectionString("DefaultConnection");

    return Fluently.Configure()
        .Database(SQLiteConfiguration.Standard
            .ConnectionString(connectionString)
            .Dialect<SQLiteDialect>()
            .Driver<SQLite20Driver>()
            .FormatSql()
            .ShowSql())
        .Mappings(m => m.FluentMappings
            .AddFromAssembly(typeof(EventMap).Assembly))
        .BuildSessionFactory();
});


builder.Services.AddScoped<NHibernate.ISession>(provider =>
    provider.GetRequiredService<ISessionFactory>().OpenSession());

builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<ITicketSalesRepository, TicketSalesRepository>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<ITicketSalesService, TicketSalesService>();

builder.Services.AddAutoMapper(typeof(MappingProfile)); 

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    using var scope = app.Services.CreateScope();
    var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
    mapper.ConfigurationProvider.AssertConfigurationIsValid();
}

app.UseHttpsRedirection();
app.UseCors("DevelopmentCors");
app.UseAuthorization();


app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapMethods("/api/{**rest}", new[] { "OPTIONS" }, () => Results.Ok())
   .RequireCors("DevelopmentCors");

app.MapEventsEndpoints();
app.MapTicketsEndpoints();

app.Run();