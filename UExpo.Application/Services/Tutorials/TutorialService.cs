using AutoMapper;
using UExpo.Domain.Entities.Tutorial;
using UExpo.Domain.Entities.Users;

namespace UExpo.Application.Services.Tutorials;

public class TutorialService : ITutorialService
{
	private ITutorialRepository _repository;
	private IMapper _mapper;

	public TutorialService(ITutorialRepository repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	public async Task<Guid> CreateAsync(TutorialDto tutorial)
	{
		return await _repository.CreateAsync(_mapper.Map<Tutorial>(tutorial));
	}

	public async Task DeleteAsync(Guid id)
	{
		await _repository.DeleteAsync(id);
	}

	public async Task<List<TutorialResponseDto>> GetAsync(UserType? type = null)
	{
		List<Tutorial> tutorials = await _repository.GetAsync(type);

		return [.. _mapper.Map<List<TutorialResponseDto>>(tutorials)
			.OrderBy(x => x.Type)
			.ThenBy(x => x.Order)];
	}

	public async Task<List<TutorialResponseDto>> GetByPageAsync(string page, UserType? type)
	{
		var tutorial = await _repository.GetAsync(x =>
			x.Page.Equals(page) &&
			(type == null || x.Type == type)
		);

		return _mapper.Map<List<TutorialResponseDto>>(tutorial).OrderBy(x => x.Order).ToList();
	}

	public async Task UpdateAsync(Guid id, TutorialDto tutorial)
	{
		var dbTutorial = await _repository.GetByIdAsync(id);

		_mapper.Map(tutorial, dbTutorial);

		await _repository.UpdateAsync(dbTutorial);
	}
}
