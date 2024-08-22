using UExpo.Domain.Dao;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Places;

public interface IPlaceRepository : IBaseRepository<PlaceDao, Place>
{
    Task<bool> AnyWithSameYearAsync(int year, Guid? id);
}
