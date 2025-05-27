using EventTicketing.DTOs.Events;
using EventTicketing.DTOs.Pagination;
using EventTicketing.Services.Events;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;

namespace EventTicketing.Endpoints.Event
{
    public static class EventsEndpoints
    {
        public static void MapEventsEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/events").WithTags("Events");

            // Get all events (paginated)
            group.MapGet("/", async (
                [AsParameters] PaginationRequest request,
                [FromServices] IEventService eventService) =>
            {
                return Results.Ok(await eventService.GetAllAsync(request));
            })
            .Produces<PagedResult<EventDto>>()
            .WithName("GetAllEvents");

            // Get upcoming events (paginated)
            group.MapGet("/upcoming/{days}", async (
                int days,
                [AsParameters] PaginationRequest request,
                [FromServices] IEventService eventService) =>
            {
                return Results.Ok(await eventService.GetUpcomingEventsAsync(days, request));
            })
            .Produces<PagedResult<EventDto>>()
            .WithName("GetUpcomingEvents");

            // Get event by ID
            group.MapGet("/{id}", async (
                string id,
                [FromServices] IEventService eventService) =>
            {
                var eventDto = await eventService.GetByIdAsync(id);
                return eventDto == null ? Results.NotFound() : Results.Ok(eventDto);
            })
            .Produces<EventDto>()
            .Produces(StatusCodes.Status404NotFound)
            .WithName("GetEventById");

            //Cache Route for event by ID
            group.MapGet("/cache/{id}", async (
               string id,
               [FromServices] IEventService eventService) =>
            {
                var eventDto = await eventService.CachedGetByIdAsync(id);
                return eventDto == null ? Results.NotFound() : Results.Ok(eventDto);
            })
           .Produces<EventDto>()
           .Produces(StatusCodes.Status404NotFound)
           .WithName("CachedGetEventById");
        }
    }
}
