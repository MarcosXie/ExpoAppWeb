using ExpoApp.Domain.Dao;
using ExpoShared.Domain.Shared;

namespace ExpoApp.Domain.Entities.Momento;


public interface IMomentoRepository : IBaseRepository<MomentoDao, Momento>
{
}
