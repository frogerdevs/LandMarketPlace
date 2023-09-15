namespace IdentityServer.Repositories.Interfaces
{
    public interface IBaseRepositoryAsync<T, in TId> where T : class
    {
        IQueryable<T> Entities { get; }

        ValueTask<T?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);

        ValueTask<List<T>> GetAllAsync(CancellationToken cancellationToken = default);
        ValueTask<List<T>> GetAllAsNoTrackingAsync(CancellationToken cancellationToken = default);

        ValueTask<IQueryable<T>> GetPagedResponseAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);

        ValueTask<T> AddAsync(T entity, CancellationToken cancellationToken = default);
        ValueTask<bool> AddRangeAsync(List<T> entity, CancellationToken cancellationToken = default);
        ValueTask<T> UpdateAsync(T entity, TId Id, CancellationToken cancellationToken = default);
        ValueTask<bool> UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        ValueTask<T> DeleteAsync(T entity, CancellationToken cancellationToken = default);
        ValueTask<bool> DeleteRangeAsync(List<T> entity, CancellationToken cancellationToken = default);
    }
}
