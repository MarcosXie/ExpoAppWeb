using ExpoShared.Domain.Entities.Exhibitors;

namespace ExpoApp.Domain.Entities.Exhibitors;

public interface IExhibitorService
{
	Task<List<ExhibitorResponseDto>> GetExhibitorsAsync(string? companyName = null);
	Task<List<string>> GetExhibitorsCompaniesAsync();
}