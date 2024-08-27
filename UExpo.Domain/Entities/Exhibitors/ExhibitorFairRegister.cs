using UExpo.Domain.Entities.Calendar.Fairs;
using UExpo.Domain.Entities.Users;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Entities.Exhibitors;

public class ExhibitorFairRegister : BaseModel
{
    public Guid ExhibitorId { get; set; }
    public User UserDao { get; set; } = null!;
    public Guid CalendarFairId { get; set; }
    public CalendarFair CalendarFair { get; set; } = null!;
    public double Value { get; set; }
    public bool IsPaid { get; set; } = false;
}
