namespace ExpoApp.Domain.Entities.Wed;

public interface IPresentService
{
	Task Create(PresentDto present);
	Task BuyPresentAsync(Guid id, BuyPresentDto dto);
	Task<List<Present>> GetPresentsAsync();
}
