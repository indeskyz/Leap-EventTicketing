using AutoMapper;
using EventTicketing.Cache.Services;
using EventTicketing.Data.Entities.Events;
using EventTicketing.Data.Repositories.Events;
using EventTicketing.DTOs.Events;
using EventTicketing.DTOs.Pagination;
using EventTicketing.Services.Base;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Threading.Tasks;


/// <summary>

// This service also provides an example of how a caching layer can be integrated.
// The caching provider allows for both in-memory and distributed caching strategies
// This also shows the advantages of using a base service class to handle common CRUD operation.

// A lot of the code you see throughout the application is meant to show reusability and extensibility of the codebase + real -world practices
// I wanted it to be a hybrid between a real-world application and a learning resource, so you can see how my thought process works.
// Sure - does anybody really need a togglable caching provider? Probably not, but it does show how I approach software design

/// </summary>


namespace EventTicketing.Services.Events
{
    public class EventService : BaseService<Event, EventDto, IEventRepository>, IEventService
    {
        protected override string CacheKeyPrefix => "events";

        public EventService(IEventRepository repository, IMapper mapper, ICacheService cache)
            : base(repository, mapper, cache)
        {
        }

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
        public override async Task<EventDto> CachedGetByIdAsync(string id)
        {
            var cacheKey = $"{CacheKeyPrefix}:byid:{id}";

            return await _cache.GetOrSetAsync(cacheKey, async () =>
            {
                var entity = await _repository.GetByIdAsync(id);

                if (entity == null) {
                    throw new KeyNotFoundException($"Event with ID {id} not found.");
                }
                return _mapper.Map<EventDto>(entity);
            }, new CacheOptions
            {
                Expiration = TimeSpan.FromHours(2),
                BypassLocalCache = false
            });
        }
    }
}
