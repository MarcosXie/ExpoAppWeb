namespace UExpo.Domain.Dao;

public class FairDao : BaseDao
{
    public string Name { get; set; } = null!;
    public bool Active { get; set; }
    public List<SegmentDao> Segments { get; set; } = [];
}
