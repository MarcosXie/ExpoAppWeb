namespace UExpo.Domain.Dao;

public class CalendarSegmentDao : BaseDao
{
    public string Name { get; set; } = null!;

    public Guid FairId { get; set; }
    public CalendarFairDao Fair { get; set; } = null!;
}

