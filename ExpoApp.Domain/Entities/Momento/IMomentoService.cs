using Microsoft.AspNetCore.Http;

namespace ExpoApp.Domain.Entities.Momento;

public interface IMomentoService
{
	Task<Guid> AddAudio(IFormFile file, Guid targetUserId);
	Task<MemoryStream> GetAudios(Guid userId, Guid id);
}
