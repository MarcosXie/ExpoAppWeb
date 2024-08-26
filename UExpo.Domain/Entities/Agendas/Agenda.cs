using UExpo.Domain.Shared;

namespace UExpo.Domain.Entities.Agendas;

public class Agenda : BaseModel
{
    public string Place { get; set; } = null!;
    public DateTime BeginDate { get; set; }
    public DateTime EndDate { get; set; }
}
