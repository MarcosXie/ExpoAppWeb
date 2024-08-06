namespace UExpo.Domain.Users;

public interface IUserRepository
{
    Task<Guid> CreateAsync(User item, CancellationToken cancellationToken = default);
    Task<List<User>> GetAsync(CancellationToken cancellationToken = default);
    Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task UpdateAsync(User item, CancellationToken cancellationToken = default);
    Task DeleteUserWithNotValidatedEmailsAsync(string email, CancellationToken cancellationToken = default);
}
