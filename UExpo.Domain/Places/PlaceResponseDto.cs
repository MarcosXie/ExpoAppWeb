namespace UExpo.Domain.Places;

public class PlaceResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public int Year { get; set; }
    public bool Active { get; set; }
}
