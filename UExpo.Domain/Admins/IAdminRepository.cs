namespace UExpo.Domain.Admins;

public interface IAdminRepository
{
    Task<Guid> CreateAsync(Admin item, CancellationToken cancellationToken = default);
    Task<Admin> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Admin?> GetByIdOrDefaultAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Admin?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}
