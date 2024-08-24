using UExpo.Domain.Dao;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Agendas;

public interface IAgendaRepository : IBaseRepository<AgendaDao, Agenda>
{
    Task<bool> HasDateInRangeAsync(DateTime beginDate, DateTime endDate, Guid? id);
}
