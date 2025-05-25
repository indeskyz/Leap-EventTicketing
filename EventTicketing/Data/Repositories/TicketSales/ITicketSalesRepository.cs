using EventTicketing.Data.Entities.Events;
using EventTicketing.Data.Entities.TicketSales;
using EventTicketing.Data.Repositories.Base;

namespace EventTicketing.Data.Repositories.Tickets
{
    public interface ITicketSalesRepository : IBaseRepository<TicketSale>
    {
        Task<(IEnumerable<TicketSale> tickets, int totalCount)> GetTicketsForEventAsync(Guid eventId, int pageNumber, int pageSize);
        Task<IEnumerable<Event>> GetTopEventsByTicketCountAsync(int count);
        Task<IEnumerable<Event>> GetTopEventsByRevenueAsync(int count);
    }
}
