namespace UExpo.Domain.FairDates;

public interface IFairDateService
{
    Task<Guid> CreateAsync(FairDateDto place);
    Task UpdateAsync(Guid id, FairDateDto place);
    Task<List<FairDateResponseDto>> GetAsync();
    Task SwitchStatusAsync(Guid id);
}
