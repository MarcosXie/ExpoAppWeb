namespace UExpo.Domain.Entities.Exhibitors;

public class ExhibitorResponseDto
{
	public Guid Id { get; set; }
	public string Enterprise { get; set; } = null!;
	public string Country { get; set; } = null!;
	public List<string> Fairs { get; set; } = [];
	public List<string> Segments { get; set; } = [];
	public string? Tags { get; set; }
	public bool HasRelationship { get; set; }
}
