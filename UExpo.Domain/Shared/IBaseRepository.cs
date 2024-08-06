namespace UExpo.Domain.Shared;

public interface IBaseRepository<TDao, TEntity>
    where TDao : class
    where TEntity : class
{
    Task<Guid> CreateAsync(TEntity item, CancellationToken cancellationToken = default);

    Task CreateAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken = default);

    Task<List<TEntity>> GetAsync(CancellationToken cancellationToken = default);

    Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<TEntity?> GetByIdOrDefaultAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Guid id, CancellationToken cancellationToken = default);

    Task UpdateAsync(TEntity item, CancellationToken cancellationToken = default);

    Task UpdateAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
