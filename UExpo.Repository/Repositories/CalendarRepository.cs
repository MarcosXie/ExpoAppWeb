using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UExpo.Domain.Dao;
using UExpo.Domain.Entities.Calendar;
using UExpo.Repository.Context;

namespace UExpo.Repository.Repositories;

public class CalendarRepository(UExpoDbContext context, IMapper mapper)
    : BaseRepository<CalendarDao, Calendar>(context, mapper), ICalendarRepository
{
    public async Task<bool> AnyLockedInSameYearAsync(int year)
    {
        return await Database.AnyAsync(x => x.Year == year && x.IsLocked);
    }

    public async Task DeleteByYearAsync(int year)
    {
        var calendars = await Database
            .AsNoTracking()
            .Where(x => x.Year!.Equals(year) && !x.IsLocked)
            .ToListAsync();

        if (calendars is null) return;

        Database.RemoveRange(calendars);

        await Context.SaveChangesAsync();
    }

    public async Task<List<Calendar>> GetByYearAsync(int year)
    {
        var calendars = await Database
            .AsNoTracking()
            .Where(x => x.Year == year)
            .ToListAsync();

        return Mapper.Map<List<Calendar>>(calendars);
    }

	public async Task<Calendar> GetNextAsync()
	{
		var calendar = await Database
			.Where(x => x.BeginDate > DateTime.Now)
			.OrderBy(x => x.BeginDate)
			.FirstOrDefaultAsync();

		return Mapper.Map<Calendar>(calendar);
	}

	public async Task<Calendar> GetNextDetailedAsync()
	{
		var query = Database
			.Where(x => x.EndDate > DateTime.Now)
			.OrderBy(x => x.EndDate);

		var tempCalendar = await query.FirstOrDefaultAsync();

		if (DateTime.Now >= tempCalendar?.BeginDate && DateTime.Now <= tempCalendar.EndDate)
		{
			var startedCalendar = await query
									.Include(x => x.Fairs)
										.ThenInclude(f => f.Segments)
									.Include(x => x.Fairs)
										.ThenInclude(f => f.FairRegisters)
											.ThenInclude(fr => fr.User)
									.FirstOrDefaultAsync();

			return Mapper.Map<Calendar>(startedCalendar);
		}

		return Mapper.Map<Calendar>(tempCalendar);
	}

	public async Task<List<int>> GetYearsAsync()
    {
        List<int> years = [];

        years.AddRange(await Database.Select(x => x.Year).Distinct().ToListAsync());
        years.AddRange(await Context.Agendas.Select(x => x.BeginDate.Year).Distinct().ToListAsync());

        return years.Distinct().ToList();
    }
}
