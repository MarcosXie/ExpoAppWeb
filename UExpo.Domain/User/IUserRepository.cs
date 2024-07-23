namespace UExpo.Domain.User;

public interface IUserRepository
{
    public Task CreateAsync(User item, CancellationToken cancellationToken = default);
    public Task<List<User>> GetAsync(CancellationToken cancellationToken = default);
    public Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public Task UpdateAsync(User item, CancellationToken cancellationToken = default);
}
