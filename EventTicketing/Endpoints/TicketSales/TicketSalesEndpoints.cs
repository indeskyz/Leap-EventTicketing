using EventTicketing.DTOs.Events;
using EventTicketing.DTOs.Pagination;
using EventTicketing.DTOs.TicketSales;
using EventTicketing.Services.Tickets;
using Microsoft.AspNetCore.Mvc;

namespace EventTicketing.Endpoints.Ticket
{
    public static class TicketSalesEndpoints
    {
        public static void MapTicketsEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/tickets").WithTags("Tickets");

            group.MapGet("/event/{eventId}", async (
                string eventId,
                [AsParameters] PaginationRequest request,
                [FromServices] ITicketSalesService ticketService) =>
            {
                return Results.Ok(await ticketService.GetTicketsForEventAsync(eventId, request));
            })
            .Produces<PagedResult<TicketSalesDto>>()
            .WithName("GetTicketsForEvent");

            group.MapGet("/top/sales", async (
                [FromServices] ITicketSalesService ticketService,
                [FromQuery] int count = PaginationConstants.DefaultTopCount) =>
            {
                return Results.Ok(await ticketService.GetTopEventsByTicketCountAsync(count));
            })
            .Produces<IEnumerable<EventSalesDto>>()
            .WithName("GetTopEventsBySales");

            group.MapGet("/top/revenue", async (
                [FromServices] ITicketSalesService ticketService,
                [FromQuery] int count = PaginationConstants.DefaultTopCount) =>
            {
                return Results.Ok(await ticketService.GetTopEventsByRevenueAsync(count));
            })
            .Produces<IEnumerable<EventSalesDto>>()
            .WithName("GetTopEventsByRevenue");
        }
    }
}