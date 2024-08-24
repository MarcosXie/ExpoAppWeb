namespace UExpo.Domain.Agendas;

public interface IAgendaService
{
    Task<Guid> CreateAsync(AgendaDto agenda);
    Task UpdateAsync(Guid id, AgendaDto agenda);
    Task<List<AgendaResponseDto>> GetAsync();
    Task DeleteAsync(Guid id);
}
