namespace UExpo.Domain.Entities.Exhibitors;

public class ExhibitorFairRegisterResponseDto
{
    public Guid Id { get; set; }
    public string FairName { get; set; } = null!;
    public string Calendar { get; set; } = null!;
    public bool IsPaid { get; set; } = false;
    public double Value { get; set; }
    public double Descount { get; set; }
    public double FinalValue { get; set; }
}
