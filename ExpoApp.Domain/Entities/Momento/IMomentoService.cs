using Microsoft.AspNetCore.Http;

namespace ExpoApp.Domain.Entities.Momento;

public interface IMomentoService
{
	Task<string> AddAudio(IFormFile file, Guid targetUserId);
	Task<MemoryStream> GetAudioFiles(Guid userId, Guid id, List<Guid> alreadyLoaded);
	Task<List<AudioResponseDto>> GetAudios(Guid userId, Guid id, List<Guid> alreadyLoaded);
	Task Delete(Guid id);
	Task UpdateComment(Guid id, string comment);
}
