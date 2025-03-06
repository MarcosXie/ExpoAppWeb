using ExpoShared.Domain.Entities.Exhibitors;

namespace ExpoApp.Domain.Entities.Exhibitors;

public class ExpoFinderResponseDto
{
	public List<ExhibitorResponseDto> Exhibitors { get; set; } = [];
	public List<string> CompanyNames { get; set; } = [];
	public List<string> Names { get; set; } = [];
	public List<string> Countries { get; set; } = [];
}