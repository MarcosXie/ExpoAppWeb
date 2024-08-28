using UExpo.Domain.Dao;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Entities.Calendar;

public interface ICalendarRepository : IBaseRepository<CalendarDao, Calendar>
{
    Task<bool> AnyLockedInSameYearAsync(int year);
    Task DeleteByYearAsync(int year);
    Task<List<Calendar>> GetByYearAsync(int year);
	Task<Calendar> GetNextAsync();
	Task<List<int>> GetYearsAsync();
}
