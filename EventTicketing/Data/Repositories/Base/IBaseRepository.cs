namespace EventTicketing.Data.Repositories.Base
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<(IEnumerable<TEntity> items, int totalCount)> GetAllAsync(int pageNumber, int pageSize);
        Task<TEntity> GetByIdAsync(int id);
    }
}