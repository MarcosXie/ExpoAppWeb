using AutoMapper;
using UExpo.Domain.Entities.Agendas;
using UExpo.Domain.Entities.Calendar;
using UExpo.Domain.Entities.Fairs;
using UExpo.Domain.Exceptions;

namespace UExpo.Application.Services.Calendars;

public class CalendarService : ICalendarService
{
    private ICalendarRepository _calendarRepository;
    private ICalendarFairRepository _calendarFairRepository;
    private IAgendaRepository _agendaRepository;
    private IFairRepository _fairRepository;
    private IMapper _mapper;

    public CalendarService(
        ICalendarRepository calendarRepository,
        ICalendarFairRepository calendarFairRepository,
        IAgendaRepository agendaRepository,
        IFairRepository fairRepository,
        IMapper mapper)
    {
        _calendarRepository = calendarRepository;
        _calendarFairRepository = calendarFairRepository;
        _agendaRepository = agendaRepository;
        _fairRepository = fairRepository;
        _mapper = mapper;
    }

    public async Task ExecuteAsync(int year)
    {
        await ValidateExecuteAsync(year);

        var agendas = await _agendaRepository.GetByYearAsync(year);
        var fairs = await _fairRepository.GetDetailedAsync();

        List<Calendar> calendars = [];

        foreach (var agenda in agendas)
        {
            var calendar = new Calendar()
            {
                BeginDate = agenda.BeginDate,
                EndDate = agenda.EndDate,
                Place = agenda.Place,
                Year = agenda.BeginDate.Year,
                IsLocked = false,
            };

            calendar.Fairs = GenerateNewFairs(fairs, calendar).ToList();

            calendars.Add(calendar);
        }

        await _calendarRepository.DeleteByYearAsync(year);

        await _calendarRepository.CreateAsync(calendars);
    }

    public async Task<List<CalendarReponseDto>> GetCalendarsAsync(int? year)
    {
        var calendars = year > 0 ?
            await _calendarRepository.GetByYearAsync((int)year) :
            await _calendarRepository.GetAsync();

        return _mapper.Map<List<CalendarReponseDto>>(calendars);
    }

    public async Task<List<CalendarFairResponseDto>> GetFairsAsync(int? year)
    {
        var fairs = year > 0 ?
            await _calendarFairRepository.GetByYearAsync((int)year) :
            await _calendarFairRepository.GetAsync();

        return MapCalendarFairs(fairs);
    }

    public async Task<List<CalendarFairResponseDto>> GetNextFairAsync(int? year)
    {
        var fairs = await _calendarFairRepository.GetNextAsync(year);

        return MapCalendarFairs(fairs);
    }

    public async Task<bool> GetIsLockedAsync(int year)
    {
        var calendars = await _calendarRepository.GetByYearAsync(year);

        return calendars.FirstOrDefault()?.IsLocked ?? false;
    }

    public async Task<List<int>> GetYearsAsync()
    {
        var years = await _calendarRepository.GetYearsAsync();

        return years;
    }

    public async Task LockAsync(int year)
    {
        var calendars = await _calendarRepository.GetByYearAsync(year);

        foreach (var calendar in calendars)
        {
            calendar.IsLocked = true;
        }

        await _calendarRepository.UpdateAsync(calendars);
    }

    #region Utils

    private async Task ValidateExecuteAsync(int year)
    {
        if (await _calendarRepository.AnyLockedInSameYearAsync(year))
            throw new BadRequestException("Already exist another calendar locked in this year!");

        if (DateTime.Now.Year > year)
            throw new BadRequestException("Cannot generate in past years!");
    }

    private static IEnumerable<CalendarFair> GenerateNewFairs(List<Fair> fairs, Calendar calendar)
    {
        foreach (var fair in fairs)
        {
            yield return new()
            {
                CalendarId = calendar.Id,
                Name = fair.Name,
                Segments = GenerateNewSegments(fair).ToList()
            };
        }
    }

    private static IEnumerable<CalendarSegment> GenerateNewSegments(Fair fair)
    {
        foreach (var segment in fair.Segments)
        {
            yield return new()
            {
                Name = segment.Name,
                FairId = fair.Id
            };
        }
    }

    private List<CalendarFairResponseDto> MapCalendarFairs(List<CalendarFair> fairs)
    {
        var mappedFairs = _mapper.Map<List<CalendarFairResponseDto>>(fairs);

        foreach (var fair in fairs)
        {
            var mappedFair = mappedFairs.First(x => x.Id == fair.Id);

            mappedFair.BeginDate = fair.Calendar.BeginDate;
            mappedFair.EndDate = fair.Calendar.EndDate;
        }

        return [.. mappedFairs.OrderBy(x => x.BeginDate).ThenBy(x => x.Name)];
    }
    #endregion
}
