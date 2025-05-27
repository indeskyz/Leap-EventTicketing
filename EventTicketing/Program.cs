using AutoMapper;
using EventTicketing.Cache.Models;
using EventTicketing.Cache.Providers;
using EventTicketing.Cache.Services;
using EventTicketing.Data.Mappings;
using EventTicketing.Data.Mappings.Events;
using EventTicketing.Data.Repositories.Events;
using EventTicketing.Data.Repositories.Tickets;
using EventTicketing.Data.Repositories.TicketSalesRepository;
using EventTicketing.Endpoints.Event;
using EventTicketing.Endpoints.Ticket;
using EventTicketing.Infrastructure;
using EventTicketing.Services.Events;
using EventTicketing.Services.Tickets;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using NHibernate;
using NHibernate.Dialect;
using NHibernate.Driver;
using StackExchange.Redis;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// Load in Env Vars and configuration settings - This loads in anything under the ConnectionStrings section of appsettings.{<ENV>}.json
builder.Services.Configure<AppSettings>(
    builder.Configuration.GetSection("ConnectionStrings"));

// <summary>
// Uncomment the block below to enable the caching layer
// </summary>

//Caching Layer - This provides an agnostic caching interface for the application with support for L1/L1+L2
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var connections = sp.GetRequiredService<IOptions<AppSettings>>().Value;
    return ConnectionMultiplexer.Connect(connections.Redis);
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis")
        ?? throw new InvalidOperationException("Redis connection string is missing");
    options.InstanceName = "EventTicketing_";
});

builder.Services.AddSingleton<ICacheProvider, MemoryCacheProvider>();
builder.Services.AddSingleton<ICacheProvider, RedisCacheProvider>();

builder.Services.AddSingleton<ICacheService, CacheService>();

builder.Services.AddMemoryCache();



// JSON Serialization Settings- This configures JSON serialization to ignore cycles and null values globally
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// CORS Policy - This allows cross-origin requests from the specified origins during development
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

// Routing Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Event Ticketing API", Version = "v1" });
});

// NHibernate Configuration for SQLite
builder.Services.AddSingleton<ISessionFactory>(provider =>
{
    var config = provider.GetRequiredService<IOptions<AppSettings>>().Value;
    return Fluently.Configure()
        .Database(SQLiteConfiguration.Standard
            .ConnectionString(config.DefaultConnection)
            .Dialect<SQLiteDialect>()
            .Driver<SQLite20Driver>())
        .Mappings(m => m.FluentMappings
            .AddFromAssembly(typeof(EventMap).Assembly))
        .BuildSessionFactory();
});



builder.Services.AddScoped<NHibernate.ISession>(provider =>
    provider.GetRequiredService<ISessionFactory>().OpenSession());


// Dependency Injection for Repositories and Services
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

// Map the endpoints & configure CORS for API routes
app.MapMethods("/api/{**rest}", new[] { "OPTIONS" }, () => Results.Ok())
   .RequireCors("DevelopmentCors");

app.MapEventsEndpoints();
app.MapTicketsEndpoints();

app.Run();