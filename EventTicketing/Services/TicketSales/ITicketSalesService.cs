using EventTicketing.Data.Entities.TicketSales;
using EventTicketing.DTOs.Events;
using EventTicketing.DTOs.Pagination;
using EventTicketing.DTOs.TicketSales;
using EventTicketing.Services.Base;

namespace EventTicketing.Services.Tickets
{
    public interface ITicketSalesService : IBaseService<TicketSale, TicketSalesDto>
    {
        Task<PagedResult<TicketSalesDto>> GetTicketsForEventAsync(Guid eventId, PaginationRequest request);
        Task<IEnumerable<EventSalesDto>> GetTopEventsByTicketCountAsync(int count);
        Task<IEnumerable<EventSalesDto>> GetTopEventsByRevenueAsync(int count);
    }
}
