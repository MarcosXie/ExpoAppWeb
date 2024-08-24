using AutoMapper;
using UExpo.Domain.Calendar;

namespace UExpo.Application.Services.Calendar;

public class CalendarService : ICalendarService
{
    private ICalendarRepository _calendarRepository;
    private ICalendarFairRepository _calendarFairRepository;
    private IMapper _mapper;

    public CalendarService(
        ICalendarRepository calendarRepository,
        ICalendarFairRepository calendarFairRepository,
        IMapper mapper)
    {
        _calendarRepository = calendarRepository;
        _calendarFairRepository = calendarFairRepository;
        _mapper = mapper;
    }

    public Task ExecuteAsync(int year)
    {
        throw new NotImplementedException();
    }

    public Task<List<CalendarReponseDto>> GetCalendarsAsync(int? year)
    {
        throw new NotImplementedException();
    }

    public Task<List<CalendarFairResponseDto>> GetFairsAsync(int? year)
    {
        throw new NotImplementedException();
    }

    public Task<bool> GetIsLockedAsync(int year)
    {
        throw new NotImplementedException();
    }

    public Task<List<CalendarFairResponseDto>> GetNextFairAsync(int? year)
    {
        throw new NotImplementedException();
    }

    public Task<int> GetYearsAsync()
    {
        throw new NotImplementedException();
    }

    public Task LockAsync(int year)
    {
        throw new NotImplementedException();
    }
}
