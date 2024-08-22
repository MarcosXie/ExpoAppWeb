namespace UExpo.Domain.Places;

public interface IPlaceService
{
    Task<Guid> CreateAsync(PlaceDto place);
    Task UpdateAsync(Guid id, PlaceDto place);
    Task<List<PlaceResponseDto>> GetAsync();
    Task UpdateStatusAsync();
}
