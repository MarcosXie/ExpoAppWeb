using AutoMapper;
using UExpo.Domain.Exceptions;
using UExpo.Domain.Places;

namespace UExpo.Application.Services.Places;

public class PlaceService : IPlaceService
{
    private IMapper _mapper;
    private IPlaceRepository _repository;

    public PlaceService(
        IPlaceRepository repository,
        IMapper mapper
    )
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<Guid> CreateAsync(PlaceDto place)
    {
        await ValidatePlaceAsync(place);

        return await _repository.CreateAsync(_mapper.Map<Place>(place));
    }

    public async Task<List<PlaceResponseDto>> GetAsync()
    {
        var places = await _repository.GetAsync();

        return [.. places.Select(_mapper.Map<PlaceResponseDto>).OrderByDescending(x => x.Year)];
    }

    public async Task UpdateAsync(Guid id, PlaceDto place)
    {
        var dbPlace = await _repository.GetByIdAsync(id);

        await ValidatePlaceAsync(place, id);

        _mapper.Map(place, dbPlace);

        await _repository.UpdateAsync(dbPlace);
    }

    public async Task UpdateStatusAsync()
    {
        var places = await _repository.GetAsync();

        var expiredPlaces = places.Where(x => x.Year < DateTime.Now.Year && x.Active).ToList();

        foreach (var expiredPlace in expiredPlaces)
        {
            expiredPlace.Active = false;
        }

        await _repository.UpdateAsync(expiredPlaces);
    }

    private async Task ValidatePlaceAsync(PlaceDto place, Guid? id = null)
    {
        if (await _repository.AnyWithSameYearAsync(place.Year, id))
            throw new BadRequestException("Exist another place with same year!");

        if (DateTime.Now.Year > place.Year)
            throw new BadRequestException("Cannot create a place in a past year!");
    }
}
