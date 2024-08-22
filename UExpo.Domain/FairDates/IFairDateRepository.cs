using UExpo.Domain.Dao;
using UExpo.Domain.Shared;

namespace UExpo.Domain.FairDates;

public interface IFairDateRepository : IBaseRepository<FairDateDao, FairDate>
{
    Task<bool> HasDateInRangeAsync(DateTime beginDate, DateTime endDate);
}
