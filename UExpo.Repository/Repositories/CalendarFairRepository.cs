using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UExpo.Domain.Calendar;
using UExpo.Domain.Dao;
using UExpo.Repository.Context;

namespace UExpo.Repository.Repositories;

public class CalendarFairRepository(UExpoDbContext context, IMapper mapper)
    : BaseRepository<CalendarFairDao, CalendarFair>(context, mapper), ICalendarFairRepository
{
    public async Task<List<CalendarFair>> GetByYearAsync(int year)
    {
        var fairs = await Database
            .AsNoTracking()
            .Include(x => x.Calendar)
            .Where(x => x.Calendar.Year == year)
            .ToListAsync();

        return Mapper.Map<List<CalendarFair>>(fairs);
    }

    public override async Task<List<CalendarFair>> GetAsync(CancellationToken cancellationToken = default)
    {
        var fairs = await Database
            .Include(x => x.Calendar)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return Mapper.Map<List<CalendarFair>>(fairs);
    }

    public async Task<List<CalendarFair>> GetNextAsync(int? year)
    {
        var fairs = await Database
            .Include(x => x.Calendar)
            .AsNoTracking()
            .Where(x => 
                (year == null || x.Calendar.Year == year)
            )
            .OrderBy(x => x.Calendar.BeginDate)
            .ToListAsync();

        fairs = [.. fairs.GroupBy(x => x.Calendar.BeginDate).First()];

        return Mapper.Map<List<CalendarFair>>(fairs);
    }
}
