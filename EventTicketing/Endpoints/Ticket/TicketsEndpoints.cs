using EventTicketing.DTOs.Events;
using EventTicketing.DTOs.Pagination;
using EventTicketing.DTOs.Tickets;
using EventTicketing.Services.Tickets;
using Microsoft.AspNetCore.Mvc;

namespace EventTicketing.Endpoints.Ticket
{
    public static class TicketsEndpoints
    {
        public static void MapTicketsEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/tickets").WithTags("Tickets");

            // Get tickets for event (paginated)
            group.MapGet("/event/{eventId}", async (
                int eventId,
                [AsParameters] PaginationRequest request,
                [FromServices] ITicketService ticketService) =>
            {
                return Results.Ok(await ticketService.GetTicketsForEventAsync(eventId, request));
            })
            .Produces<PagedResult<TicketDto>>()
            .WithName("GetTicketsForEvent");

            // Get top events by ticket sales count
            group.MapGet("/top/sales", async (
                [FromServices] ITicketService ticketService,
                [FromQuery] int count = PaginationConstants.DefaultTopCount) =>
            {
                return Results.Ok(await ticketService.GetTopEventsByTicketCountAsync(count));
            })
            .Produces<IEnumerable<EventSalesDto>>()
            .WithName("GetTopEventsBySales");

            // Get top events by revenue
            group.MapGet("/top/revenue", async (
                [FromServices] ITicketService ticketService,
                [FromQuery] int count = PaginationConstants.DefaultTopCount) =>
            {
                return Results.Ok(await ticketService.GetTopEventsByRevenueAsync(count));
            })
            .Produces<IEnumerable<EventSalesDto>>()
            .WithName("GetTopEventsByRevenue");
        }
    }
}