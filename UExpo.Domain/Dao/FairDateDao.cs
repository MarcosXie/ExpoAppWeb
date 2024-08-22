namespace UExpo.Domain.Dao;

public class FairDateDao : BaseDao
{
    public DateTime BeginDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool Active { get; set; }
}
