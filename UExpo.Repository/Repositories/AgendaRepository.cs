using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UExpo.Domain.Dao;
using UExpo.Domain.Entities.Agendas;
using UExpo.Repository.Context;

namespace UExpo.Repository.Repositories;

public class AgendaRepository(UExpoDbContext context, IMapper mapper)
    : BaseRepository<AgendaDao, Agenda>(context, mapper), IAgendaRepository
{
    public async Task<List<Agenda>> GetByYearAsync(int year)
    {
        var agendas = await Database.Where(x => x.BeginDate.Year == year).ToListAsync();

        return Mapper.Map<List<Agenda>>(agendas);
    }

    public Task<bool> HasDateInRangeAsync(DateTime beginDate, DateTime endDate, Guid? id = null)
    {
        return Database.AnyAsync(x => 
            (
				(x.BeginDate <= beginDate && x.EndDate >= beginDate) ||
				(x.BeginDate <= endDate && x.EndDate >= endDate) ||
				(x.BeginDate >= beginDate && x.EndDate <= endDate)
			) 
            &&
            (id == null || x.Id != id)
        );
    }
}
