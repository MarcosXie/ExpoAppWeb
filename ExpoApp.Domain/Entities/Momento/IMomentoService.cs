using Microsoft.AspNetCore.Http;

namespace ExpoApp.Domain.Entities.Momento;

public interface IMomentoService
{
	Task<MomentoResponseDto> AddMomentoFile(IFormFile file, Guid targetUserId, MomentoType type);
	Task<MomentoResponseDto> AddMomentoText(string value, Guid targetUserId, MomentoType type = MomentoType.Memo);
	Task<MemoryStream> GetMomentoFiles(Guid userId, Guid targetUserId, MomentoType type, List<Guid> alreadyLoaded);
	Task<List<MomentoResponseDto>> GetMomentos(Guid userId, Guid targetUserId, MomentoType type, List<Guid> alreadyLoaded);
	Task Delete(Guid id);
	Task UpdateComment(Guid id, string comment);
	Task UpdateValue(Guid id, string value);
}
