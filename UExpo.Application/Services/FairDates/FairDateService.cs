using AutoMapper;
using UExpo.Domain.FairDates;

namespace UExpo.Application.Services.FairDates;

public class FairDateService : IFairDateService
{
    private IFairDateRepository _repository;
    private IMapper _mapper;

    public FairDateService(IFairDateRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<Guid> CreateAsync(FairDateDto place)
    {
        throw new NotImplementedException();
    }

    public Task<List<FairDateResponseDto>> GetAsync()
    {
        throw new NotImplementedException();
    }

    public Task SwitchStatusAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Guid id, FairDateDto place)
    {
        throw new NotImplementedException();
    }
}
