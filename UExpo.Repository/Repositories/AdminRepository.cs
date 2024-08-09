using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UExpo.Domain.Admins;
using UExpo.Repository.Context;
using UExpo.Repository.Dao;

namespace UExpo.Repository.Repositories;

public class AdminRepository(UExpoDbContext context, IMapper mapper)
    : BaseRepository<AdminDao, Admin>(context, mapper), IAdminRepository
{
    public async Task<Admin?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var user = await Context.Admins
            .FirstOrDefaultAsync(x =>
                x.Name.ToLower().Equals(name.ToLower()), cancellationToken: cancellationToken);

        return Mapper.Map<Admin>(user);
    }

}
