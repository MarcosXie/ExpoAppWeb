namespace UExpo.Domain.Entities.Exhibitors;

public class ExhibitorResponseDto
{
	public Guid Id { get; set; }
	public string? Enterprise { get; set; }
	public required string Country { get; set; }
	public string? ProfileImage { get; set; }
	public List<string> Fairs { get; set; } = [];
	public List<string> Segments { get; set; } = [];
	public string? Tags { get; set; }
	public bool HasRelationship { get; set; }
}
