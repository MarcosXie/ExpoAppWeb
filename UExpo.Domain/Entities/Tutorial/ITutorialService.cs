using UExpo.Domain.Entities.Users;

namespace UExpo.Domain.Entities.Tutorial;

public interface ITutorialService
{
    Task<Guid> CreateAsync(TutorialDto tutorial);
    Task UpdateAsync(Guid id, TutorialDto tutorial);
    Task DeleteAsync(Guid id);
    Task<List<TutorialResponseDto>> GetAsync(TypeEnum? type = null);
}
