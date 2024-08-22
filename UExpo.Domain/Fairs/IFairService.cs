namespace UExpo.Domain.Fairs;

public interface IFairService
{
    Task<List<FairResponseDto>> GetAsync();
    Task<Guid> CreateAsync(FairDto fair);
    Task DeleteAsync(Guid id);
}
