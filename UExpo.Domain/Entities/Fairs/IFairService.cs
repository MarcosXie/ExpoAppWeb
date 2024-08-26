namespace UExpo.Domain.Entities.Fairs;

public interface IFairService
{
    Task<List<FairResponseDto>> GetAsync();
    Task<Guid> CreateAsync(FairDto fair);
    Task DeleteAsync(Guid id);
}
