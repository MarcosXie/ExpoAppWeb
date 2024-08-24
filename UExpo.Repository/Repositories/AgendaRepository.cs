using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UExpo.Domain.Agendas;
using UExpo.Domain.Dao;
using UExpo.Repository.Context;

namespace UExpo.Repository.Repositories;

public class AgendaRepository(UExpoDbContext context, IMapper mapper)
    : BaseRepository<AgendaDao, Agenda>(context, mapper), IAgendaRepository
{
    public Task<bool> HasDateInRangeAsync(DateTime beginDate, DateTime endDate, Guid? id = null)
    {
        return Database.AnyAsync(x => 
            ((x.BeginDate <= beginDate && x.EndDate >= beginDate) ||
            (x.BeginDate <= endDate && x.EndDate >= endDate)) 
            &&
            (id == null || x.Id != id)
        );
    }
}
