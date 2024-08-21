
using UExpo.Domain.Dao;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Admins;

public interface IAdminRepository : IBaseRepository<AdminDao, Admin>
{
    Task<Admin?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}
