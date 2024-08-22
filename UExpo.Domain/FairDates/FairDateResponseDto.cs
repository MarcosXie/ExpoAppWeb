namespace UExpo.Domain.FairDates;

public class FairDateResponseDto
{
    public Guid Id { get; set; }
    public DateTime BeginDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsDeletable { get; set; }
}
