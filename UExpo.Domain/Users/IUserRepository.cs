namespace UExpo.Domain.Users;

public interface IUserRepository
{
    public Task<Guid> CreateAsync(User item, CancellationToken cancellationToken = default);
    public Task<List<User>> GetAsync(CancellationToken cancellationToken = default);
    public Task<User> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
    public Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public Task UpdateAsync(User item, CancellationToken cancellationToken = default);
    public Task DeleteUserWithNotValidatedEmailsAsync(string email, CancellationToken cancellationToken = default);
}
