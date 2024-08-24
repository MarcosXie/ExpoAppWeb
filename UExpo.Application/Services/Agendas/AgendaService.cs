using AutoMapper;
using UExpo.Domain.Agendas;
using UExpo.Domain.Exceptions;

namespace UExpo.Application.Services.Agendas;

public class AgendaService : IAgendaService
{
    private IMapper _mapper;
    private IAgendaRepository _repository;

    public AgendaService(
        IAgendaRepository repository,
        IMapper mapper
    )
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<Guid> CreateAsync(AgendaDto place)
    {
        await ValidateAgendaAsync(place);

        return await _repository.CreateAsync(_mapper.Map<Agenda>(place));
    }

    public async Task DeleteAsync(Guid id)
    {
        await ValidateDeleteAsync(id);

        await _repository.DeleteAsync(id);
    }

    public async Task<List<AgendaResponseDto>> GetAsync()
    {
        var places = await _repository.GetAsync();

        return [.. places.Select(_mapper.Map<AgendaResponseDto>).OrderByDescending(x => x.BeginDate)];
    }

    public async Task UpdateAsync(Guid id, AgendaDto place)
    {
        var dbPlace = await _repository.GetByIdAsync(id);

        await ValidateAgendaAsync(place, id);

        _mapper.Map(place, dbPlace);

        await _repository.UpdateAsync(dbPlace);
    }

    private async Task ValidateAgendaAsync(AgendaDto agenda, Guid? id = null)
    {
        if (agenda.BeginDate > agenda.EndDate)
            throw new BadRequestException("The end date must be greater than begin date!");

        if (agenda.BeginDate < DateTime.Now)
            throw new BadRequestException("The begin date must be in the future");

        if (await _repository.HasDateInRangeAsync(agenda.BeginDate, agenda.EndDate, id))
            throw new BadRequestException("Already exist a configured date in this range");
    }

    private async Task ValidateDeleteAsync(Guid id)
    {
        //TODO: Adds validations
    }
}
