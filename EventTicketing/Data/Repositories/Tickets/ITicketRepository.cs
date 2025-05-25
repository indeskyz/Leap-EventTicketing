using EventTicketing.Data.Entities.Events;
using EventTicketing.Data.Entities.Tickets;
using EventTicketing.Data.Repositories.Base;

namespace EventTicketing.Data.Repositories.Tickets
{
    public interface ITicketRepository : IBaseRepository<Ticket>
    {
        Task<(IEnumerable<Ticket> tickets, int totalCount)> GetTicketsForEventAsync(int eventId, int pageNumber, int pageSize);
        Task<IEnumerable<Event>> GetTopEventsByTicketCountAsync(int count);
        Task<IEnumerable<Event>> GetTopEventsByRevenueAsync(int count);
    }
}
