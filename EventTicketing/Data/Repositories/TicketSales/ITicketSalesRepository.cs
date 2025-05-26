using EventTicketing.Data.Entities.Events;
using EventTicketing.Data.Entities.TicketSales;
using EventTicketing.Data.Repositories.Base;
using EventTicketing.DTOs.TicketSales;

namespace EventTicketing.Data.Repositories.Tickets
{
    public interface ITicketSalesRepository : IBaseRepository<TicketSale>
    {
        Task<(IEnumerable<TicketSalesDto> tickets, int totalCount)> GetTicketsForEventAsync(string eventId, int pageNumber, int pageSize);
        Task<IEnumerable<Event>> GetTopEventsByTicketCountAsync(int count);
        Task<IEnumerable<Event>> GetTopEventsByRevenueAsync(int count);
    }
}
