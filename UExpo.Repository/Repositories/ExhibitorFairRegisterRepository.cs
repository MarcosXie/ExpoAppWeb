using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UExpo.Domain.Dao;
using UExpo.Domain.Entities.Calendars.Fairs;
using UExpo.Domain.Entities.Exhibitors;
using UExpo.Domain.Entities.Users;
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
            .Where(x => x.ExhibitorId == exhibitorId && x.CalendarFair.Calendar.BeginDate >= DateTime.Now).ToListAsync();

        var paidFairs = Mapper.Map<List<ExhibitorFairRegister>>(fairs);

		var upcomingFairs = (await Context.Calendars
			.Include(x => x.Fairs)
			.AsNoTracking()
			.OrderBy(x => x.BeginDate)
			.FirstOrDefaultAsync(x => x.BeginDate >= DateTime.Now))?.Fairs ?? [];
			
		var exhibitor = await Context.Users
			.AsNoTracking()
			.FirstOrDefaultAsync(x => x.Id == exhibitorId);

		foreach(var upcomingFair in upcomingFairs)
		{
			if (!paidFairs.Any(x => x.CalendarFairId == upcomingFair.Id))
			{
				paidFairs.Add(new ExhibitorFairRegister
				{
					Id = upcomingFair.Id,
					ExhibitorId = exhibitorId,
					User = Mapper.Map<User>(exhibitor),
					CalendarFairId = upcomingFair.Id,
					CalendarFair = Mapper.Map<CalendarFair>(upcomingFair),
					IsPaid = false
				});
			}
		}

	    return paidFairs;
    }
}
