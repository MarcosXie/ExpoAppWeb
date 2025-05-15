using ExpoApp.Domain.Entities.Wed;

namespace ExpoApp.Application.Services;

public class PresentService : IPresentService
{
	private readonly IPresentRepository _presentRepository;

	public PresentService(IPresentRepository presentRepository)
	{
		_presentRepository = presentRepository;
	}
	
	public async Task Create(PresentDto present)
	{
		Present dbPresent = new()
		{
			Name = present.Name,
			Price = present.Price,
			ImageUri = present.ImageUri,
			PurchaseLink = present.PurchaseLink
		};
		
		await _presentRepository.CreateAsync(dbPresent);
	}

	public async Task BuyPresentAsync(Guid id, BuyPresentDto dto)
	{
		var present = await _presentRepository.GetByIdAsync(id);
		
		present.Buyer = dto.Name;
		present.ByPix = dto.ByPix;
		
		await _presentRepository.UpdateAsync(present);
	}

	public async Task<List<Present>> GetPresentsAsync()
	{
		return await _presentRepository.GetAsync();
	}
}
