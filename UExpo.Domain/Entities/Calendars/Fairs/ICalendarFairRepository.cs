using UExpo.Domain.Dao;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Entities.Calendars.Fairs;

public interface ICalendarFairRepository : IBaseRepository<CalendarFairDao, CalendarFair>
{
	Task<List<CalendarFair>> GetByIdsDetailedAsync(List<Guid> fairIds);
	Task<List<CalendarFair>> GetByYearAsync(int year);
	Task<List<CalendarFair>> GetNextAsync(int? year);
}
