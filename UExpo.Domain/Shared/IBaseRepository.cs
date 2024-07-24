namespace UExpo.Domain.Shared;

public interface IBaseRepository<TDao, TEntity>
    where TDao : class
    where TEntity : class
{
    public Task<Guid> CreateAsync(TEntity item, CancellationToken cancellationToken = default);

    public Task CreateAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken = default);

    public Task<List<TEntity>> GetAsync(CancellationToken cancellationToken = default);

    public Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    public Task<TEntity?> GetByIdOrDefaultAsync(Guid id, CancellationToken cancellationToken = default);

    public Task UpdateAsync(TEntity item, CancellationToken cancellationToken = default);

    public Task UpdateAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken = default);

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
