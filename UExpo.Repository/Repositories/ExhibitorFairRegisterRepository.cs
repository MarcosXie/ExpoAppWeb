using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UExpo.Domain.Dao;
using UExpo.Domain.Entities.Exhibitors;
using UExpo.Repository.Context;
namespace UExpo.Repository.Repositories;

public class ExhibitorFairRegisterRepository(UExpoDbContext context, IMapper mapper)
    : BaseRepository<ExhibitorFairRegisterDao, ExhibitorFairRegister>(context, mapper),
    IExhibitorFairRegisterRepository
{
    public async Task<List<ExhibitorFairRegister>> GetByExhibitorIdAsync(Guid exhibitorId)
    {
        var fairs = await Database
            .Include(x => x.CalendarFair)
            .ThenInclude(x => x.Calendar)
            .AsNoTracking()
            .Where(x => x.ExhibitorId == exhibitorId).ToListAsync();

        return Mapper.Map<List<ExhibitorFairRegister>>(fairs);
    }
}
