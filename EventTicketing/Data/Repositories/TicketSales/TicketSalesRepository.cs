using EventTicketing.Data.Entities.Events;
using EventTicketing.Data.Entities.TicketSales;
using EventTicketing.Data.Repositories.Base;
using EventTicketing.Data.Repositories.Tickets;
using EventTicketing.DTOs.TicketSales;
using NHibernate.Linq;
using NHibernate.Transform;
using System.Text.Json;
using ISession = NHibernate.ISession;

namespace EventTicketing.Data.Repositories.TicketSalesRepository
{
    public class TicketSalesRepository : BaseRepository<TicketSale>, ITicketSalesRepository
    {
        public TicketSalesRepository(ISession session) : base(session)
        {
        }

        public async Task<(IEnumerable<TicketSalesDto> tickets, int totalCount)> GetTicketsForEventAsync(string eventId, int pageNumber, int pageSize)
        {
            var query = _session.Query<TicketSale>()
                .Where(t => t.EventId == eventId)
                .OrderBy(t => t.Price);

            var totalCount = await query.CountAsync();

            var tickets = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new TicketSalesDto
                {
                    Id = t.Id,
                    EventId = t.EventId,
                    Price = t.Price,
                    Type = "Standard",
                    QuantityAvailable = 0,
                    QuantitySold = 1
                })
                .ToListAsync();

            return (tickets, totalCount);
        }

        public async Task<IEnumerable<Event>> GetTopEventsByTicketCountAsync(int count)
        {
            return await _session.Query<TicketSale>()
                .GroupBy(t => t.Event)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetTopEventsByRevenueAsync(int count)
        {
            return await _session.Query<TicketSale>()
                .GroupBy(t => t.Event)
                .OrderByDescending(g => g.Sum(t => t.Price))
                .Select(g => g.Key)
                .Take(count)
                .ToListAsync();
        }

    }
}