using UExpo.Domain.Dao;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Fairs.FairDates;

public interface IFairDateRepository : IBaseRepository<FairDateDao, FairDate>
{
    Task<bool> HasDateInRangeAsync(DateTime beginDate, DateTime endDate);
}
