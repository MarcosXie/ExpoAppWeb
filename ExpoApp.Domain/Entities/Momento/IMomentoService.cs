using Microsoft.AspNetCore.Http;

namespace ExpoApp.Domain.Entities.Momento;

public interface IMomentoService
{
	Task<string> AddAudio(IFormFile file, Guid targetUserId);
	Task<MemoryStream> GetAudios(Guid userId, Guid id);
}
