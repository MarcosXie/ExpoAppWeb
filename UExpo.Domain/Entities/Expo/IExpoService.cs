namespace UExpo.Domain.Entities.Expo;

public interface IExpoService
{
	Task<ExpoResponseDto> GetCurrentExpoAsync();
}
