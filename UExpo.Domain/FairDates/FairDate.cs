using UExpo.Domain.Shared;

namespace UExpo.Domain.FairDates;

public class FairDate : BaseModel
{
    public DateTime BeginDate { get; set; }
    public DateTime EndDate { get; set; }
}
