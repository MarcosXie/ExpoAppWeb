using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UExpo.Domain.Dao;
using UExpo.Repository.Context;

public class UExpoUserStore : IUserStore<UserDao>, IUserPasswordStore<UserDao>
{
    private readonly UExpoDbContext _context;

    public UExpoUserStore(UExpoDbContext context)
    {
        _context = context;
    }

    public async Task<IdentityResult> CreateAsync(UserDao user, CancellationToken cancellationToken)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> DeleteAsync(UserDao user, CancellationToken cancellationToken)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);
        return IdentityResult.Success;
    }

    public async Task<UserDao> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        return await _context.Users.FindAsync(new object[] { int.Parse(userId) }, cancellationToken);
    }

    public async Task<UserDao> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Name.ToUpper() == normalizedUserName, cancellationToken) ?? default;
    }

    public Task<string> GetNormalizedUserNameAsync(UserDao user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.Name.ToUpper());
    }

    public Task<string> GetUserIdAsync(UserDao user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.Id.ToString());
    }

    public Task<string> GetUserNameAsync(UserDao user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.Name);
    }

    public Task SetNormalizedUserNameAsync(UserDao user, string normalizedName, CancellationToken cancellationToken)
    {
        // Normalized names are not stored separately in this implementation.
        return Task.CompletedTask;
    }

    public Task SetUserNameAsync(UserDao user, string userName, CancellationToken cancellationToken)
    {
        user.Name = userName;
        return Task.CompletedTask;
    }

    public async Task<IdentityResult> UpdateAsync(UserDao user, CancellationToken cancellationToken)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
        return IdentityResult.Success;
    }

    public void Dispose()
    {
        // Dispose the context if necessary
    }

    public Task SetPasswordHashAsync(UserDao user, string passwordHash, CancellationToken cancellationToken)
    {
        user.Password = passwordHash;
        return Task.CompletedTask;
    }

    public Task<string> GetPasswordHashAsync(UserDao user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.Password);
    }

    public Task<bool> HasPasswordAsync(UserDao user, CancellationToken cancellationToken)
    {
        return Task.FromResult(!string.IsNullOrEmpty(user.Password));
    }
}
