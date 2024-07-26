using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UExpo.Domain.Users;
using UExpo.Repository.Context;
using UExpo.Repository.Dao;

namespace UExpo.Repository.Repositories;

public class UserRepository(UExpoDbContext context, IMapper mapper)
    : BaseRepository<UserDao, User>(context, mapper), IUserRepository
{
    public async Task<User> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var userDao = await Database.FirstOrDefaultAsync(x => x.Email.ToLower().Equals(email));

        return Mapper.Map<User>(userDao);
    }
}
