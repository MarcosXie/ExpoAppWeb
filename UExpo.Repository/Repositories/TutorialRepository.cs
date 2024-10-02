using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UExpo.Domain.Dao;
using UExpo.Domain.Entities.Tutorial;
using UExpo.Domain.Entities.Users;
using UExpo.Repository.Context;

namespace UExpo.Repository.Repositories;

public class TutorialRepository(UExpoDbContext context, IMapper mapper)
    : BaseRepository<TutorialDao, Tutorial>(context, mapper), ITutorialRepository
{
    public async Task<List<Tutorial>> GetAsync(UserType? type)
    {
        var tutorials = await Database.Where(t => type == null || t.Type == type).ToListAsync();

        return Mapper.Map<List<Tutorial>>(tutorials);
    }
}
