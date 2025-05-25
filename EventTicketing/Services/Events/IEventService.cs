using EventTicketing.Data.Entities.Events;
using EventTicketing.DTOs.Events;
using EventTicketing.DTOs.Pagination;
using EventTicketing.Services.Base;

namespace EventTicketing.Services.Events
{
    public interface IEventService : IBaseService<Event, EventDto>
    {
        Task<PagedResult<EventDto>> GetUpcomingEventsAsync(int days, PaginationRequest request);
    }
}
