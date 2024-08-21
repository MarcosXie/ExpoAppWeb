using UExpo.Domain.Dao;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Users;

public interface IUserRepository : IBaseRepository<UserDao, User>
{
    Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task DeleteUserWithNotValidatedEmailsAsync(string email, CancellationToken cancellationToken = default);
}
