using AutoMapper;
using UExpo.Domain.Entities.Calendar;
using UExpo.Domain.Entities.Calendar.Fairs;
using UExpo.Domain.Entities.Catalogs;
using UExpo.Domain.Entities.Exhibitors;
using UExpo.Domain.Entities.Expo;
using UExpo.Domain.Entities.Users;

namespace UExpo.Application.Services.Expos;

public class ExpoService : IExpoService
{
	private ICalendarRepository _calendarRepository;
	private ICatalogRepository _catalogRepository;
	private IUserRepository _userRepository;
	private IMapper _mapper;

	public ExpoService(
		ICalendarRepository calendarRepository, 
		ICatalogRepository catalogRepository,
		IUserRepository userRepository,
		IMapper mapper)
	{
		_calendarRepository = calendarRepository;
		_catalogRepository = catalogRepository;
		_userRepository = userRepository;
		_mapper = mapper;
	}

	public async Task<ExpoResponseDto> GetCurrentExpoAsync()
	{
		var calendar = await _calendarRepository.GetNextDetailedAsync();

		return await CreateExpoResponseAsync(calendar);
	}

	public async Task<List<ExhibitorResponseDto>> GetExhibitorsAsync(ExpoSearchDto searchDto)
	{
		List<User> users = await _userRepository.GetAsync(searchDto);

		return users.Select(x => new ExhibitorResponseDto()
		{
			Id = x.Id,
			Country = x.Country,
			Enterprise = x.Enterprise ?? string.Empty,
			Tags = x.Catalog?.Tags
		}).ToList();
	}

	private async Task<ExpoResponseDto> CreateExpoResponseAsync(Calendar calendar)
	{
		ExpoResponseDto expo = new()
		{
			CalendarId = calendar.Id,
			Place = calendar.Place,
			BeginDate = calendar.BeginDate,
			EndDate = calendar.EndDate,
			IsStarted = calendar.BeginDate < DateTime.Now
		};

		if (expo.IsStarted)
		{
			expo.Fairs = _mapper.Map<List<CalendarFairOptionResponseDto>>(calendar.Fairs);
			expo.Segments = expo.Fairs.SelectMany(f => f.Segments).ToList();
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
							Tags = register.User.Catalog?.Tags
						});
					}
				}
			}
		}

		return expo;
	}
}
