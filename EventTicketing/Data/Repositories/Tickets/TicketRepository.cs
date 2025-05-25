using EventTicketing.Data.Entities.Events;
using EventTicketing.Data.Entities.Tickets;
using EventTicketing.Data.Repositories.Base;
using NHibernate.Linq;
using ISession = NHibernate.ISession;

namespace EventTicketing.Data.Repositories.Tickets
{
    public class TicketRepository : BaseRepository<Ticket>, ITicketRepository
    {
        public TicketRepository(ISession session) : base(session)
        {
        }

        public async Task<(IEnumerable<Ticket> tickets, int totalCount)> GetTicketsForEventAsync(int eventId, int pageNumber, int pageSize)
        {
            var query = _session.Query<Ticket>()
                .Where(t => t.Event.Id == eventId)
                .OrderBy(t => t.Price);

            var totalCount = await query.CountAsync();

            var tickets = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (tickets, totalCount);
        }

        public async Task<IEnumerable<Event>> GetTopEventsByTicketCountAsync(int count)
        {
            return await _session.Query<Ticket>()
                .GroupBy(t => t.Event)
                .OrderByDescending(g => g.Sum(t => t.QuantitySold))
                .Select(g => g.Key)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetTopEventsByRevenueAsync(int count)
        {
            return await _session.Query<Ticket>()
                .GroupBy(t => t.Event)
                .OrderByDescending(g => g.Sum(t => t.Price * t.QuantitySold))
                .Select(g => g.Key)
                .Take(count)
                .ToListAsync();
        }
    }
}
