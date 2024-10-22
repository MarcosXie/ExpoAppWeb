using AutoMapper;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using UExpo.Application.Utils;
using UExpo.Domain.Entities.Calendars;
using UExpo.Domain.Entities.Calendars.Fairs;
using UExpo.Domain.Entities.Catalogs;
using UExpo.Domain.Entities.Exhibitors;
using UExpo.Domain.Exceptions;

namespace UExpo.Application.Services.CalendarFairs;

public class CalendarFairService : ICalendarFairService
{
    private readonly ICalendarFairRepository _calendarFairRepository;
	private readonly ICalendarRepository _calendarRepository;
	private readonly AuthUserHelper _authUserHelper;
    private readonly IExhibitorFairRegisterRepository _fairRegisterRepository;
    private readonly ICatalogService _catalogService;
    private readonly IConfiguration _config;
    private readonly IMapper _mapper;

    public CalendarFairService(
        ICalendarFairRepository calendarFairRepository,
		ICalendarRepository calendarRepository,
        IExhibitorFairRegisterRepository exhibitorFairRegisterRepository,
        ICatalogService catalogService,
        IMapper mapper,
        IConfiguration config,
        AuthUserHelper authUserHelper)
    {
        _calendarFairRepository = calendarFairRepository;
		_calendarRepository = calendarRepository;
		_authUserHelper = authUserHelper;
        _fairRegisterRepository = exhibitorFairRegisterRepository;
        _catalogService = catalogService;
        _config = config;
        _mapper = mapper;
    }

    public async Task<List<CalendarFairResponseDto>> GetFairsAsync(int? year)
    {
        var fairs = year > 0 ?
            await _calendarFairRepository.GetByYearAsync((int)year) :
            await _calendarFairRepository.GetAsync();

        return MapCalendarFairs(fairs);
    }

    public async Task<List<ExhibitorFairRegisterResponseDto>> GetRegisteredFairsAsync()
    {
        var exhibitorId = _authUserHelper.GetUser().Id;

        List<ExhibitorFairRegister> fairs = await _fairRegisterRepository.GetByExhibitorIdAsync(exhibitorId);

        return MapExhibitorFairRegisterResponse(fairs).ToList();
    }

    public async Task<List<CalendarFairResponseDto>> GetUpcomingFairsAsync(int? year)
    {
        var fairs = await _calendarFairRepository.GetNextAsync(year);

        return MapCalendarFairs(fairs);
    }

    public async Task<List<CalendarFairOptionResponseDto>> GetUpcomingFairsOptionsAsync()
    {
        var fairs = await _calendarFairRepository.GetNextAsync(null);

        return _mapper.Map<List<CalendarFairOptionResponseDto>>(fairs);
    }

    public async Task<Guid> RegisterAsync(Guid calendarFairId)
    {
        var calendarFair = await _calendarFairRepository.GetByIdAsync(calendarFairId);
        var exhibitorId = _authUserHelper.GetUser().Id;

        await ValidateRegisterAsync(calendarFair, exhibitorId);

        ExhibitorFairRegister fairRegister = new()
        {
            CalendarFairId = calendarFair.Id,
            ExhibitorId = exhibitorId,
            Value = double.Parse(_config.GetRequiredSection("PaymentInfo:FairPrice").Value!),
            IsPaid = false,
        };

        return await _fairRegisterRepository.CreateAsync(fairRegister);
    }
    public async Task<bool> PayAsync(List<Guid> fairIds)
    {
		var fairs = await _calendarFairRepository.GetByIdsAsync(fairIds);
		var exhibitorId = _authUserHelper.GetUser().Id;

		List<ExhibitorFairRegister> fairRegisters = [];
		var descount = double.Parse(_config.GetSection("PaymentInfo:FairDescount").Value!, CultureInfo.InvariantCulture);
		var value = double.Parse(_config.GetSection("PaymentInfo:FairPrice").Value!, CultureInfo.InvariantCulture);

		foreach (var fair in fairs)
		{
			fairRegisters.Add(new()
			{
				CalendarFairId = fair.Id,
				ExhibitorId = exhibitorId,
				Value = Math.Round(value * (1 - descount / 100), 2),
				IsPaid = true,
			});
		}

		await _fairRegisterRepository.CreateAsync(fairRegisters);

        return true;
    }

	public async Task<string> GetNextExpoDateAsync()
	{
		var calendar = await _calendarRepository.GetNextAsync();
		return BuildCalendarName(calendar);
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

    private IEnumerable<ExhibitorFairRegisterResponseDto> MapExhibitorFairRegisterResponse(List<ExhibitorFairRegister> registers)
    {
        foreach (var register in registers)
        {
            var descount = double.Parse(_config.GetSection("PaymentInfo:FairDescount").Value!, CultureInfo.InvariantCulture);
			var value = double.Parse(_config.GetSection("PaymentInfo:FairPrice").Value!, CultureInfo.InvariantCulture);

			if (descount > 100) descount = 100;
            if (descount < 0) descount = 0;

            var calendar = register.CalendarFair.Calendar;

            yield return new()
            {
                Id = register.Id,
                FairName = register.CalendarFair.Name,
                IsPaid = register.IsPaid,
                Value = value,
                Descount = descount,
                Calendar = BuildCalendarName(calendar),
                FinalValue = Math.Round(value * (1 - descount / 100), 2),
            };
        }
    }

    private static string BuildCalendarName(Domain.Entities.Calendars.Calendar calendar)
    {
		if (calendar is null) return "";
        return $"{calendar.Place} {calendar.Year} {calendar.BeginDate.Month}/{calendar.BeginDate.Day} - {calendar.EndDate.Month}/{calendar.EndDate.Day}";
    }

    private async Task ValidateRegisterAsync(CalendarFair calendarFair, Guid exhibitorId)
    {
        var fairs = await _fairRegisterRepository.GetByExhibitorIdAsync(exhibitorId);

        if (fairs.Any(x => x.CalendarFairId == calendarFair.Id))
            throw new BadRequestException("You are already registered in this fair!");
    }
}
