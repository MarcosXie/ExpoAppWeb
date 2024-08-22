namespace UExpo.Domain.Dao;

public class PlaceDao : BaseDao
{
    public string Name { get; set; } = null!;
    public int Year { get; set; }
    public bool Active { get; set; }
}
