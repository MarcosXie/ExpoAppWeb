namespace UExpo.Domain.Fairs.FairDates;

public interface IFairDateService
{
    Task<Guid> CreateAsync(FairDateDto place);
    Task<List<FairDateResponseDto>> GetAsync();
    Task DeleteAsync(Guid id);
}
