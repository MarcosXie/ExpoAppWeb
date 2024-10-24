using UExpo.Domain.Dao;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Entities.Calendars;

public interface ICalendarRepository : IBaseRepository<CalendarDao, Calendar>
{
	Task<bool> AnyLockedInSameYearAsync(int year);
	Task DeleteByYearAsync(int year);
	Task<List<Calendar>> GetByYearAsync(int year);
	Task<Calendar> GetNextAsync();
	Task<Calendar> GetNextDetailedAsync(bool returnFullResultEver = false);
	Task<List<int>> GetYearsAsync();
}
