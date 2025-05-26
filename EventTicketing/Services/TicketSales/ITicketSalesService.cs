using EventTicketing.Data.Entities.TicketSales;
using EventTicketing.DTOs;
using EventTicketing.DTOs.Events;
using EventTicketing.DTOs.Pagination;
using EventTicketing.DTOs.TicketSales;
using EventTicketing.Services.Base;

namespace EventTicketing.Services.Tickets
{
    public interface ITicketSalesService : IBaseService<TicketSale, TicketSalesDto>
    {
        Task<PagedResult<TicketSalesDto>> GetTicketsForEventAsync(string eventId, PaginationRequest request);
        Task<ApiResponse<IEnumerable<EventSalesDto>>> GetTopEventsByTicketCountAsync(int count);
        Task<ApiResponse<IEnumerable<EventSalesDto>>> GetTopEventsByRevenueAsync(int count);

    }
}
