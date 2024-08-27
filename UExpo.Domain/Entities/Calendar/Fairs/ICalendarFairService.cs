using UExpo.Domain.Entities.Exhibitors;

namespace UExpo.Domain.Entities.Calendar.Fairs;

public interface ICalendarFairService
{
    Task<List<CalendarFairResponseDto>> GetFairsAsync(int? year);
    Task<List<CalendarFairResponseDto>> GetUpcomingFairsAsync(int? year);
    Task<List<CalendarFairOptionResponseDto>> GetUpcomingFairsOptionsAsync();
    Task<Guid> RegisterAsync(Guid calendarFairId);
    Task<List<ExhibitorFairRegisterResponseDto>> GetRegisteredFairsAsync();
    Task<bool> PayAsync(List<Guid> fairRegisterIds);
}
