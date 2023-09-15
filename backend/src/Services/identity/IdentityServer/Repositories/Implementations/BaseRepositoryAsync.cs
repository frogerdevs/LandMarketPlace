using IdentityServer.Data;
using IdentityServer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Repositories.Implementations
{
    public class BaseRepositoryAsync<T, TId> : IBaseRepositoryAsync<T, TId> where T : class
    {
        private readonly ApplicationDbContext _dbContext;
        public BaseRepositoryAsync(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<T> Entities => _dbContext.Set<T>();
        public async ValueTask<List<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await new ValueTask<List<T>>(_dbContext
                .Set<T>().ToListAsync(cancellationToken: cancellationToken));
        }
        public async ValueTask<List<T>> GetAllAsNoTrackingAsync(CancellationToken cancellationToken = default)
        {
            return await new ValueTask<List<T>>(_dbContext
                .Set<T>().AsNoTracking().ToListAsync(cancellationToken: cancellationToken));
        }
        public async ValueTask<T?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<T>().FindAsync(new object?[] { id }, cancellationToken: cancellationToken);
        }
        public ValueTask<IQueryable<T>> GetPagedResponseAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            return new ValueTask<IQueryable<T>>(_dbContext
                .Set<T>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking());
        }
        public async ValueTask<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _dbContext.Set<T>().AddAsync(entity, cancellationToken);
            await SaveAsync(cancellationToken);
            return entity;
        }
        public async ValueTask<bool> AddRangeAsync(List<T> entity, CancellationToken cancellationToken = default)
        {
            bool res;
            try
            {
                await _dbContext.Set<T>().AddRangeAsync(entity, cancellationToken);
                await SaveAsync(cancellationToken);
                res = true;
            }
            catch
            {
                res = false;
            }
            return res;
        }
        public async ValueTask<T> UpdateAsync(T entity, TId Id, CancellationToken cancellationToken = default)
        {
            T? exist = await _dbContext.Set<T>().FindAsync(new object?[] { Id }, cancellationToken: cancellationToken);
            _dbContext.Entry(exist!).CurrentValues.SetValues(entity);
            await SaveAsync(cancellationToken);
            return entity;
        }
        public async ValueTask<bool> UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            bool res;
            try
            {
                _dbContext.UpdateRange(entities, cancellationToken);
                await SaveAsync(cancellationToken);
                res = true;
            }
            catch
            {
                res = false;
            }
            return res;
        }
        public async ValueTask<T> DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            _dbContext.Set<T>().Remove(entity);
            await SaveAsync(cancellationToken);
            return entity;
        }
        public async ValueTask<bool> DeleteRangeAsync(List<T> entity, CancellationToken cancellationToken = default)
        {
            bool res;
            try
            {
                _dbContext.Set<T>().RemoveRange(entity);
                await SaveAsync(cancellationToken);
                res = true;
            }
            catch
            {
                res = false;
            }
            return res;
        }
        private async Task SaveAsync(CancellationToken token = default)
        {
            await _dbContext.SaveChangesAsync(token);
        }
    }
}
