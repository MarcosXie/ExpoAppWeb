using UExpo.Domain.Entities.Catalogs.CatalogSegments;
using UExpo.Domain.Entities.Catalogs;
using UExpo.Domain.Entities.Calendars.Fairs;
using AutoMapper;
using UExpo.Domain.Entities.Calendars.Segments;
using UExpo.Domain.Entities.Calendars;
using UExpo.Domain.Entities.Tags;
using UExpo.Domain.Entities.Users;

namespace UExpo.Application.Services.Tags;

public class TagService : ITagService
{
	private readonly ICatalogRepository _repository;
	private readonly ICalendarRepository _calendarRepository;
	private readonly ICalendarFairRepository _calendarFairRepository;
	private readonly ICatalogSegmentRepository _catalogSegmentRepository;
	private readonly ICalendarSegmentRepository _calendarSegmentRepository;
	private readonly IUserRepository _userRepository;
	private readonly IMapper _mapper;

	public TagService(
		ICatalogRepository repository,
		ICalendarFairRepository calendarFairRepository,
		ICalendarSegmentRepository calendarSegmentRepository,
		ICalendarRepository calendarRepository,
		ICatalogSegmentRepository catalogSegmentRepository,
		IUserRepository userRepository,
		IMapper mapper)
	{
		_repository = repository;
		_calendarRepository = calendarRepository;
		_calendarFairRepository = calendarFairRepository;
		_catalogSegmentRepository = catalogSegmentRepository;
		_calendarSegmentRepository = calendarSegmentRepository;
		_userRepository = userRepository;
		_mapper = mapper;
	}

	public async Task<CatalogTagSegmentsResponseDto> GetTagsAsync(Guid id)
	{
		var user = await _userRepository.GetByIdDetailedAsync(id);

		var catalog = await _repository.GetByIdDetailedAsync(user.Catalog!.Id);
		var calendar = await _calendarRepository.GetNextDetailedAsync(true);

		CatalogTagSegmentsResponseDto response = new()
		{
			Tags = catalog.Tags,
			Fairs = _mapper.Map<List<CalendarFairOptionResponseDto>>(calendar.Fairs.Where(x =>
				x.FairRegisters.Any(fr => fr.User.Id == catalog.UserId)
			))
		};

		response.Segments = response.Fairs.SelectMany(f => f.Segments).ToList();

		foreach (var segment in catalog.Segments.Where(x => x.CalendarId == calendar.Id))
		{
			var resSegment = response.Segments.FirstOrDefault(s => s.Id == segment.CalendarSegmentId);

			if (resSegment is not null)
			{
				resSegment.IsSelected = true;
			}
		}

		return response;
	}

	public async Task UpdateTagsAsync(Guid id, string tags)
	{
		tags = tags.ToLower();
		var user = await _userRepository.GetByIdDetailedAsync(id);

		var catalog = await _repository.GetByIdAsync(user.Catalog!.Id);

		catalog.Tags = tags;

		await _repository.UpdateTagsAsync(catalog);
	}

	public async Task UpdateSegmentsAsync(Guid id, List<Guid> segmentIds, string tags)
	{
		await UpdateTagsAsync(id, tags);

		var user = await _userRepository.GetByIdDetailedAsync(id);

		var catalog = await _repository.GetByIdDetailedAsync(user.Catalog!.Id);
		var calendar = await _calendarRepository.GetNextDetailedAsync(true);

		var currentSegments = catalog.Segments.Where(x => x.CalendarId == calendar.Id).ToList();
		var currentSegmentsIds = currentSegments.Select(x => x.CalendarSegmentId).ToList();

		var segmentsToDelete = currentSegments.Where(x => !segmentIds.Contains(x.CalendarSegmentId)).ToList();
		var segmentIdsToAdd = segmentIds.Where(x => !currentSegmentsIds.Contains(x)).ToList();

		await _catalogSegmentRepository.DeleteAsync(segmentsToDelete.Select(x => x.Id).ToList());

		var segmentsToAdd = await _calendarSegmentRepository.GetByIdsAsync(segmentIdsToAdd);

		List<CatalogSegment> catalogSegmentToAdd = [];

		foreach (var segment in segmentsToAdd)
		{
			catalogSegmentToAdd.Add(new()
			{
				CalendarId = calendar.Id,
				CatalogId = catalog.Id,
				CalendarSegmentId = segment.Id,
			});
		}

		await _catalogSegmentRepository.CreateAsync(catalogSegmentToAdd);
	}
}
