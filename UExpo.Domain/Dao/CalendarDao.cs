using UExpo.Domain.Dao.Shared;

namespace UExpo.Domain.Dao;

public class CalendarDao : BaseDao
{
    public string Place { get; set; } = null!;
    public int Year { get; set; }
    public DateTime BeginDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<CalendarFairDao> Fairs { get; set; } = [];
    public bool IsLocked { get; set; } = false;
	public List<RelationshipDao> Relationships { get; set; } = [];
}
