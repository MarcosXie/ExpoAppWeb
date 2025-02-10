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
	public async Task<string> AddAudio(IFormFile file)
	{
		var fileName = GenerateMomentoFileName("audio");
		var uri = await fileStorageService.UploadPrivateFileAsync(file, fileName, FileStorageKeys.MomentoFiles);

		var momento = new Momento
		{
			UserId = authUserHelper.GetUser().Id,
			Type = MomentoType.Audio,
			Value = uri
		};
		
		await momentoRepository.CreateAsync(momento);
		
		return uri;
	}

	public async Task<List<MemoryStream>> GetAudios(Guid id)
	{
		var audios = await momentoRepository.GetAsync(x => x.UserId == id);
		
		return await fileStorageService.GetFilesAsync(
			audios.Select(x => x.Value).ToList(),
			FileStorageKeys.MomentoFiles
		);
	}

	private string GenerateMomentoFileName(string fileType)
	{
		var userId = authUserHelper.GetUser().Id;
		Random random = new();
		return GetFileName(fileType, userId.ToString(), "-", random.Next(10000, 99999).ToString());
	}

	private static string GetFileName(string name, params string[] ids)
	{
		string prefix = string.Join('-', ids);

		return $"{prefix}-{Path.GetFileName(name)}";
	}
}