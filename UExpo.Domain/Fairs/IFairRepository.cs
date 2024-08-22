using UExpo.Domain.Dao;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Fairs;

public interface IFairRepository : IBaseRepository<FairDao, Fair>
{
    Task<bool> AnyWithSameNameAsync(string name);
}
