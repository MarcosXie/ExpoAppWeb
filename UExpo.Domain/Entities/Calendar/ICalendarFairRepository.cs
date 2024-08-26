using UExpo.Domain.Dao;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Entities.Calendar;

public interface ICalendarFairRepository : IBaseRepository<CalendarFairDao, CalendarFair>
{
    Task<List<CalendarFair>> GetByYearAsync(int year);
    Task<List<CalendarFair>> GetNextAsync(int? year);
}
