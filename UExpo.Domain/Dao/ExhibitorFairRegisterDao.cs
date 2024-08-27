namespace UExpo.Domain.Dao;

public class ExhibitorFairRegisterDao : BaseDao
{
    public Guid ExhibitorId { get; set; }
    public UserDao User { get; set; } = null!;
    public Guid CalendarFairId { get; set; }
    public CalendarFairDao CalendarFair { get; set; } = null!;
    public double Value { get; set; }
    public bool IsPaid { get; set; } = false;
}

