using AutoMapper;
using EventTicketing.Data.Repositories.Base;
using EventTicketing.DTOs.Pagination;

namespace EventTicketing.Services.Base
{
    public abstract class BaseService<TEntity, TDto, TRepository> : IBaseService<TEntity, TDto>
      where TEntity : class
      where TDto : class
      where TRepository : IBaseRepository<TEntity>
    {
        protected readonly TRepository _repository;
        protected readonly IMapper _mapper;
        
        protected BaseService(TRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
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
    }
}