using AutoMapper;
using UExpo.Domain.Entities.Fairs;
using UExpo.Domain.Exceptions;

namespace UExpo.Application.Services.Fairs;

public class FairService : IFairService
{
    private IFairRepository _repository;
    private IMapper _mapper;

    public FairService(IFairRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Guid> CreateAsync(FairDto fair)
    {
        await ValidateFairAsync(fair);

        return await _repository.CreateAsync(_mapper.Map<Fair>(fair));
    }

    public async Task<List<FairResponseDto>> GetAsync()
    {
        var fairs = await _repository.GetAsync();

        return [.. fairs.Select(_mapper.Map<FairResponseDto>).OrderByDescending(x => x.Name)];
    }

    public async Task DeleteAsync(Guid id)
    {
        var fair = await _repository.GetByIdAsync(id);

        if (fair.Active)
            throw new BadRequestException("Cannot delete an active fair");

        await _repository.DeleteAsync(id);
    }

    private async Task ValidateFairAsync(FairDto fair)
    {
        if (await _repository.AnyWithSameNameAsync(fair.Name))
            throw new BadRequestException("Already exists a fair with the same name!");
    }
}
