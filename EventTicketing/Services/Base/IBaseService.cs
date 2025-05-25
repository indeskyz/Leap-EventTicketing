using EventTicketing.DTOs.Pagination;

namespace EventTicketing.Services.Base
{
    public interface IBaseService<TEntity, TDto> where TEntity : class where TDto : class
    {
        Task<PagedResult<TDto>> GetAllAsync(PaginationRequest request);
        Task<TDto> GetByIdAsync(int id);
    }
}