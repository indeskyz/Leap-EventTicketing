using EventTicketing.Data.Entities.Events;
using EventTicketing.Data.Repositories.Base;

namespace EventTicketing.Data.Repositories.Events
{
    public interface IEventRepository : IBaseRepository<Event>
    {
        Task<(IEnumerable<Event> events, int totalCount)> GetUpcomingEventsAsync(DateTime cutoffDate, int pageNumber, int pageSize);
    }
}
