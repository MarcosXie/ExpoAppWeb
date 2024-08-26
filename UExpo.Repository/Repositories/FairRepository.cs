using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UExpo.Domain.Dao;
using UExpo.Domain.Entities.Fairs;
using UExpo.Repository.Context;

namespace UExpo.Repository.Repositories;

public class FairRepository(UExpoDbContext context, IMapper mapper)
    : BaseRepository<FairDao, Fair>(context, mapper), IFairRepository
{
    public async Task<bool> AnyWithSameNameAsync(string name)
    {
        return await Database.AnyAsync(x => x.Name.Equals(name));
    }

    public async Task<List<Fair>> GetDetailedAsync()
    {
        var fairs = await Database.Include(x => x.Segments).ToListAsync();

        return Mapper.Map<List<Fair>>(fairs);
    }
}
