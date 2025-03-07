namespace ExpoApp.Domain.Entities.Exhibitors;

public class ExpoFinderOptionsResponseDto
{
	public List<string> CompanyNames { get; set; } = [];
	public List<string> Emails { get; set; } = [];
	public List<string> Countries { get; set; } = [];
}