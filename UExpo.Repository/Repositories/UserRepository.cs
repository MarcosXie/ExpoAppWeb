using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UExpo.Domain.Dao;
using UExpo.Domain.Users;
using UExpo.Repository.Context;

namespace UExpo.Repository.Repositories;

public class UserRepository(UExpoDbContext context, IMapper mapper)
    : BaseRepository<UserDao, User>(context, mapper), IUserRepository
{
    public async Task DeleteUserWithNotValidatedEmailsAsync(string email, CancellationToken cancellationToken = default)
    {
        IQueryable<UserDao> invalidEmails = Database.Where(x => x.Email.ToLower().Equals(email.ToLower()) && !x.IsEmailValidated);

        if (!invalidEmails.Any()) return;

        Database.RemoveRange(invalidEmails);

        await Context.SaveChangesAsync(cancellationToken);
    }

    public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        UserDao? userDao = await Database.FirstOrDefaultAsync(x => x.Email.ToLower().Equals(email), cancellationToken: cancellationToken);

        return Mapper.Map<User>(userDao);
    }
}
