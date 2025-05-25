using EventTicketing.Data.Entities.Events;
using EventTicketing.Data.Repositories.Base;
using NHibernate.Linq;
using ISession = NHibernate.ISession;


namespace EventTicketing.Data.Repositories.Events
{
    public class EventRepository : BaseRepository<Event>, IEventRepository
    {
        public EventRepository(ISession session) : base(session)
        {
        }

        public async Task<(IEnumerable<Event> events, int totalCount)> GetUpcomingEventsAsync(DateTime cutoffDate, int pageNumber, int pageSize)
        {
            var query = _session.Query<Event>()
                .Where(e => e.StartsOn >= DateTime.UtcNow && e.StartsOn <= cutoffDate)
                .OrderBy(e => e.StartsOn);

            var totalCount = await query.CountAsync();

            var events = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (events, totalCount);
        }
    }

}
