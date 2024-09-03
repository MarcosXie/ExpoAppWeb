namespace UExpo.Domain.Entities.Exhibitors;

public class ExhibitorResponseDto
{
	public Guid Id { get; set; }
	public string Enterprise { get; set; } = null!;
	public string Country { get; set; } = null!;
	public string? Tags { get; set; }
}
