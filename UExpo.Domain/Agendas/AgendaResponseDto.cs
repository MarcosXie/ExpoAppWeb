namespace UExpo.Domain.Agendas;

public class AgendaResponseDto
{
    public Guid Id { get; set; }
    public string Place { get; set; } = null!;
    public DateTime BeginDate { get; set; }
    public DateTime EndDate { get; set; }
}
