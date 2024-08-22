namespace UExpo.Domain.Fairs;

public class FairResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public bool Active { get; set; }
}
