using EventTicketing.Data.Entities.Tickets;
using EventTicketing.DTOs.Events;
using EventTicketing.DTOs.Pagination;
using EventTicketing.DTOs.Tickets;
using EventTicketing.Services.Base;

namespace EventTicketing.Services.Tickets
{
    public interface ITicketService : IBaseService<Ticket, TicketDto>
    {
        Task<PagedResult<TicketDto>> GetTicketsForEventAsync(int eventId, PaginationRequest request);
        Task<IEnumerable<EventSalesDto>> GetTopEventsByTicketCountAsync(int count);
        Task<IEnumerable<EventSalesDto>> GetTopEventsByRevenueAsync(int count);
    }
}
