using AutoMapper;
using UExpo.Application.Utils;
using UExpo.Domain.Entities.Calendars;
using UExpo.Domain.Entities.Calendars.Fairs;
using UExpo.Domain.Entities.Catalogs;
using UExpo.Domain.Entities.Exhibitors;
using UExpo.Domain.Entities.Expo;
using UExpo.Domain.Entities.Relationships;
using UExpo.Domain.Entities.Users;

namespace UExpo.Application.Services.Expos;

public class ExpoService : IExpoService
{
	private ICalendarRepository _calendarRepository;
	private ICatalogRepository _catalogRepository;
	private IUserRepository _userRepository;
	private IRelationshipRepository _relationshipRepository;
	private AuthUserHelper _authUserHelper;
	private IMapper _mapper;

	public ExpoService(
		ICalendarRepository calendarRepository, 
		ICatalogRepository catalogRepository,
		IUserRepository userRepository,
		IRelationshipRepository relationshipRepository,
		AuthUserHelper authUserHelper,
		IMapper mapper)
	{
		_calendarRepository = calendarRepository;
		_catalogRepository = catalogRepository;
		_userRepository = userRepository;
		_relationshipRepository = relationshipRepository;
		_authUserHelper = authUserHelper;
		_mapper = mapper;
	}

	public async Task<ExpoResponseDto> GetCurrentExpoAsync()
	{
		var calendar = await _calendarRepository.GetNextDetailedAsync();
		var relationships = await GetUserRelationshipsAsync();

		return await CreateExpoResponseAsync(calendar, relationships);
	}

	public async Task<List<ExhibitorResponseDto>> GetExhibitorsAsync(ExpoSearchDto searchDto)
	{
		List<User> users = await _userRepository.GetAsync(searchDto);
		List<Relationship> relationships = await GetUserRelationshipsAsync();

		return users.Select(x => new ExhibitorResponseDto()
		{
			Id = x.Id,
			Country = x.Country,
			Enterprise = x.Enterprise ?? string.Empty,
			Tags = searchDto.Tags.Count > 0 ? string.Join(',', x.Catalog!.Tags.Split(',').Where(tag => searchDto.Tags.Contains(tag.ToLower()))) : x.Catalog!.Tags,
			HasRelationship = relationships.Any(r => r.SupplierUserId == x.Id)
		}).ToList();
	}

	private async Task<List<Relationship>> GetUserRelationshipsAsync()
	{
		return await _relationshipRepository.GetByUserIdAsync(_authUserHelper.GetUser().Id);
	}

	private async Task<ExpoResponseDto> CreateExpoResponseAsync(Calendar calendar, List<Relationship> relationships)
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
							Tags = register.User.Catalog?.Tags,
							HasRelationship = relationships.Any(r => r.SupplierUserId == register.User.Id),
							ProfileImage = register.User.ProfileImageUri,
							Fairs = calendar.Fairs.Where(x => x.CalendarId == calendar.Id && x.FairRegisters.Any(fr => fr.ExhibitorId == register.ExhibitorId)).Select(x => x.Name.ToLower()).ToList(),
							Segments = register.User.Catalog!.Segments.Where(x => x.CalendarId == calendar.Id).Select(x => x.CalendarSegment.Name.ToLower()).ToList(),
						});
					}
				}
			}
		}

		return expo;
	}
}
