using UExpo.Domain.Dao;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Entities.Fairs;

public interface IFairRepository : IBaseRepository<FairDao, Fair>
{
    Task<bool> AnyWithSameNameAsync(string name);
    Task<List<Fair>> GetDetailedAsync();
}
