using ExpoApp.Domain.Entities.Momento;
using ExpoShared.Application.Utils;
using ExpoShared.Domain.FileStorage;
using Microsoft.AspNetCore.Http;

namespace ExpoApp.Application.Services;

public class MomentoService(
	IFileStorageService fileStorageService,
	AuthUserHelper authUserHelper,
	IMomentoRepository momentoRepository
) : IMomentoService
{
	public async Task<Guid> AddAudio(IFormFile file, Guid targetUserId)
	{
		var momento = new Momento
		{
			UserId = authUserHelper.GetUser().Id,
			TargetUserId = targetUserId,
			Type = MomentoType.Audio,
			Value = ""
		};
		
		var fileName = GetFileName("audio.mp4", momento.Id.ToString());
		
		momento.Value = fileName;
		
		momento.Value = await fileStorageService.UploadPrivateFileAsync(file, fileName, FileStorageKeys.MomentoFiles);

		await momentoRepository.CreateAsync(momento);
		
		return momento.Id;
	}

	public async Task<MemoryStream> GetAudios(Guid userId, Guid targetUserId)
	{
		var audios = await momentoRepository.GetAsync(x => x.UserId == userId && x.TargetUserId == targetUserId);
		
		return await fileStorageService.GetFilesAsync(
			audios.Select(x => x.Value).ToList(),
			FileStorageKeys.MomentoFiles
		);
	}
	
	private static string GetFileName(string name, params string[] ids)
	{
		string prefix = string.Join('-', ids);

		return $"{prefix}-{Path.GetFileName(name)}";
	}
}