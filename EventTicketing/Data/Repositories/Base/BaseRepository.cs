using NHibernate.Linq;
using ISession = NHibernate.ISession;

namespace EventTicketing.Data.Repositories.Base
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected readonly ISession _session;

        protected BaseRepository(ISession session)
        {
            _session = session;
        }

        public async Task<(IEnumerable<TEntity> items, int totalCount)> GetAllAsync(int pageNumber, int pageSize)
        {
            var query = _session.Query<TEntity>();
            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<TEntity> GetByIdAsync(string id)
        {
            return await _session.GetAsync<TEntity>(id);
        }

    }
}