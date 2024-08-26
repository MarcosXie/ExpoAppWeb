using UExpo.Domain.Dao;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Entities.Agendas;

public interface IAgendaRepository : IBaseRepository<AgendaDao, Agenda>
{
    Task<List<Agenda>> GetByYearAsync(int year);
    Task<bool> HasDateInRangeAsync(DateTime beginDate, DateTime endDate, Guid? id);
}
