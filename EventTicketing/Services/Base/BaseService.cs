using AutoMapper;
using EventTicketing.Cache.Services;
using EventTicketing.Data.Repositories.Base;
using EventTicketing.DTOs.Pagination;
using NHibernate.Dialect.Schema;

namespace EventTicketing.Services.Base
{
    public abstract class BaseService<TEntity, TDto, TRepository> : IBaseService<TEntity, TDto>
      where TEntity : class
      where TDto : class
      where TRepository : IBaseRepository<TEntity>
    {
        protected readonly TRepository _repository;
        protected readonly IMapper _mapper;
        protected readonly ICacheService _cache;
        protected abstract string CacheKeyPrefix { get; }
        protected BaseService(TRepository repository, IMapper mapper, ICacheService cache)
        {
            _repository = repository;
            _mapper = mapper;
            _cache = cache;
        }

        public virtual async Task<PagedResult<TDto>> GetAllAsync(PaginationRequest request)
        {
            var (items, totalCount) = await _repository.GetAllAsync(request.PageNumber, request.PageSize);
            var itemsDto = _mapper.Map<IEnumerable<TDto>>(items);

            return new PagedResult<TDto>
            {
                Items = itemsDto,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        public virtual async Task<TDto> GetByIdAsync(string id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return _mapper.Map<TDto>(entity);
        }

        public virtual async Task<TDto> CachedGetByIdAsync(string id)
        {
            var cacheKey = $"{CacheKeyPrefix}:byid:{id}";

            return await _cache.GetOrSetAsync(cacheKey, async () =>
            {
                var entity = await _repository.GetByIdAsync(id);
                return _mapper.Map<TDto>(entity);
            }, new CacheOptions { Expiration = TimeSpan.FromHours(1) });

        }

        public virtual async Task InvalidateCacheByItemId(string id)
        {
            var individualKey = $"{CacheKeyPrefix}:byid:{id}";
            await _cache.RemoveAsync(individualKey);
        }
    }
}