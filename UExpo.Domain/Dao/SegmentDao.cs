namespace UExpo.Domain.Dao;

public class SegmentDao : BaseDao
{
    public string Name { get; set; } = null!;

    public Guid FairId { get; set; }
    public FairDao Fair { get; set; } = null!;
}
