using AutoMapper;
using UExpo.Domain.Entities.Expo;

namespace UExpo.Application.Services.Expos;

public class LastSearchedTagsService : ILastSearchedTagsService
{
	private ILastSearchedTagsRepository _repository;
	private IMapper _mapper;

	public LastSearchedTagsService(ILastSearchedTagsRepository repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	public async Task<LastSearchedTagsResponseDto?> GetByUserIdAsync(Guid userId)
	{
		var lsTags = await _repository.GetByUserIdOrDefaultAsync(userId);

		return lsTags is not null ? _mapper.Map<LastSearchedTagsResponseDto>(lsTags) : default; 
	}

	public async Task UpdateAsync(Guid userId, LastSearchedTagsDto lastSearchedTags)
	{
		var lsTags = await _repository.GetByUserIdOrDefaultAsync(userId);

		if (lsTags is null)
		{
			var mappedLsTags = _mapper.Map<LastSearchedTags>(lastSearchedTags);

			mappedLsTags.UserId = userId;

			await _repository.CreateAsync(mappedLsTags);
			return;
		}

		_mapper.Map(lastSearchedTags, lsTags);

		await _repository.UpdateAsync(lsTags);
	}
}

