using UExpo.Domain.Entities.Exhibitors;

namespace UExpo.Domain.Entities.Expo;

public interface IExpoService
{
	Task<ExpoResponseDto> GetCurrentExpoAsync();
	Task<List<ExhibitorResponseDto>> GetExhibitorsAsync(ExpoSearchDto searchDto);
}
