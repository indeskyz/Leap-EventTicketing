# Event Ticketing System – Server API

Full Stack Developer Position - Leap Event Technology

There are 2 branches

`tanner-dev` --> does not require redis as there is no cache implementation

`cache-layer` --> requires you to have a redis server

The cache-layer has some extra services, tests, and an endpoint to show you how I would setup a service that can connect and query any type of caching mechanism. Whether its HybridCache, SystemCache, Cloud resource using redis, local redis, etc.

App still runs the exact same and you can query an event by Id and have its result saved in the redis cache!

---

## Project Layout - Server
```
├── DTOs/                     # Data transfer objects
├── Data/
│   ├── Mappings/             # NHibernate mappings (Fluent)
│   ├── Repositories/         # Repository interfaces and implementations
├── Endpoints/                # Minimal API route declarations
├── Middleware/               # Error handling middleware
├── Services/                 # Business logic layer
├── Properties/
├── appsettings.json          # Production configuration
├── appsettings.Development.json # Development overrides
├── Program.cs                # Application startup and DI setup
├── EventTicketing.csproj
├── Cache/                    # Cache Layer - PoC how an agnostic L1/L1+L2 Service could look like
├── DatabaseSetup.md          # Instructions to set up your SQLite database (see below)
```

## Assumptions

- The schema is read-only for this application unless otherwise noted.

- SQLite is used as the primary relational data store.

- Redis has been configured but not yet implemented (caching layer planned). App was constructed with the thought of it being the base layer for the project inside of Caching.md. You can test the cache endpoint that is provided, just be sure to uncomment the `Caching Layer` block inside of Program.cs!

- All services should be extensible with interfaces and base classes to support future growth.

- Monetary conversions can be handled via the backend using simple conversion mappings to keep legacy columns in-tact.(Would recommend transferring from SQLite to something such as PostgreSQL or even SQL Server for the benefits of having more scoped value types for our columns as SQLite only offers 5 storage classes ). In the current DB there are GUID's being stored as TEXT. Not the biggest issue but it makes mapping from application code to database values tricky sometimes + its better to have a proper matching type like how in PSQL you can use a dedicated UUID type which helps the db and app know what is being stored underneath and you _can_ sometimes get preformance benefits, in terms of storage & retrieval plus validation.

- Client Side will handle the restriction of `n` days to query by for Events (30, 90, 180, etc). This logic stays on the frontend as the server only cares about being able to paginate the request. Why? The methods were built to be as generic as possible. If you want to lock down query params they can be stripped before we even make the request.

---

# Event Ticketing System – Server API

## Getting Started

### Prerequisites

* [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
* [SQLite](https://www.sqlite.org/download.html) (CLI optional)
* Redis (optional – caching not yet used)

---
## Development Notes

* Ensure Redis is running if you plan to use it (currently unused but its set it to easily use it).
* Swagger and seeding only ran in development.
* CORS is limited to local development hosts --> update your URLs accordingly in Program.cs .

```
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevelopmentCors", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "https://localhost:5173", "some-new-url")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

```

* The NHibernate session is scoped per request.
* AutoMapper config validation is run at startup in development mode.

### Configuration

This project uses both `appsettings.json` for production and `appsettings.Development.json` for local development.

To configure the application:

1. Clone the repo
2. Ensure your database is setup – **see [`DatabaseSetup.md`](./DatabaseSetup.md)**
3. Create or modify `appsettings.Development.json` as needed:

**NOTE THE DOUBLE SLASHES IF YOU ARE RUNNING INTO ISSUES CHECK YOUR CONN STRING**

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=Data Source=D:\\Leap\\EventTicketing\\skillsAssessmentEvents.db",
    "Redis": "localhost:6379"
  },
  "Database": {
    "AutoMigrate": true,
    "SeedTestData": true
  }
}
```

4. Run the application:

```bash
dotnet build
dotnet run
```

---

## API Endpoints

### Events

* `GET /api/events?days=30|60|180`
  Returns upcoming events within the specified window.

* `GET /api/tickets/cache/{eventId}`
  Returns all tickets for a given event ID Using the PoC Cache - **Client is not exposed to this endpoint**

### Tickets

* `GET /api/tickets/{eventId}`
  Returns all tickets for a given event ID.

* `GET /api/tickets/top5/count`
  Top 5 events by number of ticket sales.

* `GET /api/tickets/top5/revenue`
  Top 5 events by total revenue.

---

## Features & Middleware

* **NHibernate** for ORM access with fluent mappings
* **AutoMapper** for DTO transformation
* **Swagger UI** at `/swagger` (enabled in development)
* **CORS** enabled for local development
* **Global error handling middleware**
* **Redis** cache service (unregistered but fully configured)


---

## Unit Tests

Unit tests are located in the corresponding `Tests/` folder Tests use **NUnit** and **Moq**.

They validate:

* EventService behavior
* TicketSalesService behavior
* CacheService behavior

---

## Architecture and Code Organization

### Why use .NET Minimal API?

The application uses ASP.NET Core Minimal APIs to keep the codebase:

- **Lightweight and fast to start:** Minimal APIs eliminate boilerplate controller code, reducing overhead and speeds up development.
- **Clear routing:** Endpoint definitions are concise and colocated, making it easy to see which routes exist.
- **Flexible:** allow for easy injection of dependencies and middleware + additional features can be dropped right in with little to no overhead changes being required. 


### Code Organization

The project is architected in a layered, modular way to support maintainability and extensibility:

- **Endpoints:** Minimal API route handlers are grouped by domain (Events, Tickets) in separate extension methods. They act as the HTTP interface.
- **Services:** Business logic is encapsulated in services (`IEventService`, `ITicketSalesService`), exposing clean, testable interfaces.
- **Repositories:** Data access is isolated behind repository interfaces, implemented with NHibernate to handle ORM and DB interaction.
- **DTOs:** Data Transfer Objects are used for shaping API responses and requests, decoupling internal domain models from external contracts.
- **Middleware:** Centralized cross-cutting concerns (error handling, logging) are implemented as middleware.
- **Configuration:** The app uses a combination of `appsettings.json` + environment overrides to make application configuration extensible.

### Dependency Injection and Testing

- [All services and repositories are registered via DI container with scoped lifetimes](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-9.0#lifetime-and-registration-options).
- NHibernate `ISessionFactory` and `ISession` are injected to provide unit-of-work per request.
- AutoMapper profiles centralize mapping logic and are validated at startup.
- Layered approach allows for ease when doing unit and integration testing by mocking dependencies.

---