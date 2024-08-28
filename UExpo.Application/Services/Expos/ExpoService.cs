using AutoMapper;
using UExpo.Domain.Entities.Calendar;
using UExpo.Domain.Entities.Calendar.Fairs;
using UExpo.Domain.Entities.Catalogs;
using UExpo.Domain.Entities.Expo;

namespace UExpo.Application.Services.Expos;

public class ExpoService : IExpoService
{
	private ICalendarRepository _calendarRepository;
	private ICatalogRepository _catalogRepository;
	private IMapper _mapper;

	public ExpoService(
		ICalendarRepository calendarRepository, 
		ICatalogRepository catalogRepository, 
		IMapper mapper)
	{
		_calendarRepository = calendarRepository;
		_catalogRepository = catalogRepository;
		_mapper = mapper;
	}

	public async Task<ExpoResponseDto> GetCurrentExpoAsync()
	{
		var calendar = await _calendarRepository.GetNextDetailedAsync();

		return await CreateExpoResponseAsync(calendar);
	}

	private async Task<ExpoResponseDto> CreateExpoResponseAsync(Calendar calendar)
	{
		ExpoResponseDto expo = new()
		{
			BeginDate = calendar.BeginDate,
			EndDate = calendar.EndDate,
			IsStarted = calendar.BeginDate < DateTime.Now
		};

		if (expo.IsStarted)
		{
			expo.Fairs = _mapper.Map<List<CalendarFairOptionResponseDto>>(calendar.Fairs);
			expo.Tags = await _catalogRepository.GetAllTagsAsync();
			foreach (var fair in calendar.Fairs) 
			{
				foreach(var register in fair.FairRegisters)
				{
					if (!expo.Exhibitors.Any(x => x.Id == register.User.Id))
					{
						expo.Exhibitors.Add(new()
						{
							Id = register.User.Id,
							Country = register.User.Country,
							Enterprise = register.User.Enterprise ?? string.Empty,
						});
					}
				}
			}
		}

		return expo;
	}
}
