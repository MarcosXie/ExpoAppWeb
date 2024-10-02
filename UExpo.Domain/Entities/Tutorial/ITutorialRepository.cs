using UExpo.Domain.Dao;
using UExpo.Domain.Entities.Users;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Entities.Tutorial;

public interface ITutorialRepository : IBaseRepository<TutorialDao, Tutorial>
{
    Task<List<Tutorial>> GetAsync(UserType? type);
}
