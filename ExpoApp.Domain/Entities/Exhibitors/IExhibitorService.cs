using ExpoShared.Domain.Entities.Exhibitors;

namespace ExpoApp.Domain.Entities.Exhibitors;

public interface IExhibitorService
{
	Task<List<ExhibitorResponseDto>> GetExhibitorsAsync(
		string? companyName = null, 
		string? email = null, 
		string? country = null
	);
	Task<ExpoFinderOptionsResponseDto> GetFinderOptionsAsync(
		string? companyName = null,
		string? email = null,
		string? country = null);
}