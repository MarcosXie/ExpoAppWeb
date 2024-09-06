namespace UExpo.Domain.Entities.Calendars;

public interface ICalendarService
{
	Task<List<int>> GetYearsAsync();
	Task<bool> GetIsLockedAsync(int year);
	Task ExecuteAsync(int year);
	Task LockAsync(int year);
	Task<List<CalendarResponseDto>> GetCalendarsAsync(int? year);

}
