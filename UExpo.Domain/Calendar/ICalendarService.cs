namespace UExpo.Domain.Calendar;

public interface ICalendarService
{
    Task<int> GetYearsAsync();
    Task<bool> GetIsLockedAsync(int year);
    Task ExecuteAsync(int year);
    Task LockAsync(int year);
    Task<List<CalendarReponseDto>> GetCalendarsAsync(int? year);
    Task<List<CalendarFairResponseDto>> GetFairsAsync(int? year);
    Task<List<CalendarFairResponseDto>> GetNextFairAsync(int? year);
}
