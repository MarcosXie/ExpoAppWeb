namespace UExpo.Domain.Agendas;

public class AgendaDto
{
    public string Place { get; set; } = null!;
    public DateTime BeginDate { get; set; }
    public DateTime EndDate { get; set; }
}
