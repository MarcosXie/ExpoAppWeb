using UExpo.Domain.Dao;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Calendar;

public interface ICalendarRepository : IBaseRepository<CalendarDao, Calendar>
{
    Task<bool> AnyLockedInSameYearAsync(int year);
    Task DeleteByYearAsync(int year);
    Task<List<Calendar>> GetByYearAsync(int year);
    Task<List<int>> GetYearsAsync();
}
