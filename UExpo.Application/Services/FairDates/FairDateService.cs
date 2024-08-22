using AutoMapper;
using UExpo.Domain.Exceptions;
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

    public async Task<Guid> CreateAsync(FairDateDto date)
    {
        await ValidateDateAsync(date);

        return await _repository.CreateAsync(_mapper.Map<FairDate>(date));
    }

    public async Task<List<FairDateResponseDto>> GetAsync()
    {
        var dates = await _repository.GetAsync();

        return [..
                    dates.Select(date =>
                    {
                        var mappedDate = _mapper.Map<FairDateResponseDto>(date);

                        // TODO: Add validation
                        mappedDate.IsDeletable = DateTime.Now < mappedDate.BeginDate;

                        return mappedDate;
                    })
            .OrderByDescending(x => x.BeginDate)];
    }

    public async Task DeleteAsync(Guid id)
    {
        var date = await _repository.GetByIdAsync(id);

        await ValidateDeleteAsync(date);

        await _repository.DeleteAsync(id);
    }

    private async Task ValidateDateAsync(FairDateDto date)
    {
        if (date.BeginDate > date.EndDate)
            throw new BadRequestException("The end date must be greater than begin date!");

        if (date.BeginDate < DateTime.Now)
            throw new BadRequestException("The begin date must be in the future");

        if (await _repository.HasDateInRangeAsync(date.BeginDate, date.EndDate))
            throw new BadRequestException("Already exist a configured date in this range");
    }

    private async Task ValidateDeleteAsync(FairDate date)
    {
        //TODO: Implement validation
    }
}
