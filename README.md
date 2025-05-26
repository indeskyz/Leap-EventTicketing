# Event Ticketing System – Server API

An ASP.NET Core Web API (.NET 8) for browsing, querying, and analyzing event ticketing data. This backend powers the EventTicketing Client and exposes endpoints for upcoming events, ticket sales, and event performance analytics.

---

## Assumptions

- The schema is read-only for this application unless otherwise noted.
- SQLite is used as the primary relational data store.
- NHibernate is the chosen ORM for flexibility with advanced queries and mapping.
- Redis is configured but not yet implemented (caching layer planned).
- All services should be extensible with interfaces and base classes to support future growth.
- Monetary Versions can be handled via the backend using simple conversion mappings to keep legacy columns in tact
- Client Side will handle the restriction of `n` days to query by for Events (30, 90, 180, etc). This logic stays on the frontend as the server only cares about being able to paginate the request.

---

# Event Ticketing System – Server API

## Getting Started

### Prerequisites

* [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
* [SQLite](https://www.sqlite.org/download.html) (CLI optional)
* Redis (optional – caching not yet used)

---

### Configuration

This project uses both `appsettings.json` for production and `appsettings.Development.json`.

To configure the application:

1. Clone the repo
2. Ensure your database is setup – **see [`DatabaseSetup.md`](./DatabaseSetup.md)**
3. Create or modify `appsettings.Development.json` as needed:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=YOUR_PATH/skillsAssessmentEvents.db",
    "Redis": "localhost:6379"
  },
  "Database": {
    "AutoMigrate": true,
    "SeedTestData": true
  }
}
```

> ⚠️ Development seeding should **never** be enabled in production.

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
* **CORS** enabled for local development on `localhost:5173`
* **Global error handling middleware**
* **Redis** cache service registered (not used yet)

---

## Development Notes

* Ensure Redis is running if you plan to use it (currently unused).
* Swagger and seeding only run in development.
* CORS is limited to local development hosts.
* The NHibernate session is scoped per request.
* AutoMapper config validation is run at startup in development mode.

---

## Unit Tests

Unit tests are located in the corresponding `Tests/` folder (not shown above if not yet created). They validate:

* EventService behavior
* TicketSalesService behavior
* NHibernate integration (mocked/faked)
* Mapping configurations

---


## Architecture and Code Organization

### Why Minimal APIs?

The application uses ASP.NET Core Minimal APIs to keep the codebase:

- **Lightweight and fast to start:** Minimal APIs eliminate boilerplate controller code, reducing overhead and speeding up development.
- **Clear routing:** Endpoint definitions are concise and colocated, making it easy to see which routes exist.
- **Flexible:** Minimal APIs allow easy injection of dependencies and middleware, while still supporting all features of ASP.NET Core.
- **Modern and future-proof:** Minimal APIs are the recommended approach in .NET 8 for simple REST services, aligning with Microsoft’s vision.

### Code Organization

The project is architected in a layered, modular way to support maintainability and extensibility:

- **Endpoints:** Minimal API route handlers are grouped by domain (Events, Tickets) in separate extension methods. They act as the HTTP interface.
- **Services:** Business logic is encapsulated in services (`IEventService`, `ITicketSalesService`), exposing clean, testable interfaces.
- **Repositories:** Data access is isolated behind repository interfaces, implemented with NHibernate to handle ORM and DB interaction.
- **DTOs:** Data Transfer Objects are used for shaping API responses and requests, decoupling internal domain models from external contracts.
- **Middleware:** Centralized cross-cutting concerns (error handling, logging) are implemented as middleware.
- **Configuration:** The use of `appsettings.json` + environment overrides and dependency injection ensures configuration and dependencies are managed cleanly.

### Dependency Injection and Testing

- All services and repositories are registered via DI container with scoped lifetimes.
- NHibernate `ISessionFactory` and `ISession` are injected to provide unit-of-work per request.
- AutoMapper profiles centralize mapping logic and are validated at startup.
- This layered approach enables easy unit and integration testing by mocking dependencies.

---

## Design Considerations

### Extensibility

To allow for future schema expansion or provider swapping (e.g., moving from SQLite to SQL Server):

- **Services are interface-driven** (`IEventService`, `ITicketSalesService`)
- **Repositories are layered abstractions** over NHibernate (`IEventRepository`, etc.)
- **DTOs** are used to shield consumers from internal schema changes.
- **AutoMapper** ensures transformation logic is centralized and testable.
- **Middleware** is used for global error handling.
- **Configuration** is separated across `appsettings.json` and `appsettings.Development.json` for environment-specific behavior.

### Performance

- Queries for "Top 5 Events" are optimized for both count-based and revenue-based ranking.
- NHibernate is configured to format and show SQL for visibility during development.
- Pagination and date filtering ensure efficient lookups.

---

## Project Layout

.
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
├── DatabaseSetup.md          # Instructions to set up your SQLite database (see below)
