namespace UExpo.Domain.Dao;

public class AgendaDao : BaseDao
{
    public string Place { get; set; } = null!;
    public DateTime BeginDate { get; set; }
    public DateTime EndDate { get; set; }
}
