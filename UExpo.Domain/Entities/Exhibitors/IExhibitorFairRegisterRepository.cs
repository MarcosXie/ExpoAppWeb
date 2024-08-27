using UExpo.Domain.Dao;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Entities.Exhibitors;

public interface IExhibitorFairRegisterRepository
    : IBaseRepository<ExhibitorFairRegisterDao, ExhibitorFairRegister>
{
    Task<List<ExhibitorFairRegister>> GetByExhibitorIdAsync(Guid exhibitorId);
}
