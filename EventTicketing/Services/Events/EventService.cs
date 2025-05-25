using AutoMapper;
using EventTicketing.Data.Entities.Events;
using EventTicketing.Data.Repositories.Events;
using EventTicketing.DTOs.Events;
using EventTicketing.DTOs.Pagination;
using EventTicketing.Services.Base;

namespace EventTicketing.Services.Events
{
    public class EventService(IEventRepository repository, IMapper mapper)
    : BaseService<Event, EventDto, IEventRepository>(repository, mapper), IEventService
    {

        public async Task<PagedResult<EventDto>> GetUpcomingEventsAsync(int days, PaginationRequest request)
        {
            if (days <= 0)
            {
                throw new ArgumentException("Days must be a positive number", nameof(days));
            }

            var cutoffDate = DateTime.UtcNow.AddDays(days);
            var (events, totalCount) = await _repository.GetUpcomingEventsAsync(cutoffDate, request.PageNumber, request.PageSize);

            return new PagedResult<EventDto>
            {
                Items = _mapper.Map<IEnumerable<EventDto>>(events),
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
    }
}
