using AutoMapper;
using EventTicketing.Data.Mappings;
using EventTicketing.Data.Mappings.Events;
using EventTicketing.Data.Repositories.Events;
using EventTicketing.Data.Repositories.Tickets;
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
using Microsoft.Extensions.Caching.StackExchangeRedis;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "EventTicketing_";
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
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<ITicketService, TicketService>();

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
app.UseAuthorization();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapEventsEndpoints();
app.MapTicketsEndpoints();

app.Run();