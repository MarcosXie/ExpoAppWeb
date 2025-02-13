using ExpoApp.Domain.Entities.Momento;
using ExpoShared.Application.Utils;
using ExpoShared.Domain.FileStorage;
using Microsoft.AspNetCore.Http;
using Task = System.Threading.Tasks.Task;

namespace ExpoApp.Application.Services;

public class MomentoService(
	IFileStorageService fileStorageService,
	AuthUserHelper authUserHelper,
	IMomentoRepository momentoRepository
) : IMomentoService
{
	public async Task<string> AddAudio(IFormFile file, Guid targetUserId)
	{
		var userId = authUserHelper.GetUser().Id;
		var momentos = await momentoRepository.GetAsync(x => 
			x.UserId == userId 
			&& x.TargetUserId == targetUserId
			&& x.Type == MomentoType.Audio
		);

		var recordNumber = momentos.Any() ? momentos.Max(x => x.Order) + 1 : 1;
		
		var momento = new Momento
		{
			UserId = userId,
			TargetUserId = targetUserId,
			Type = MomentoType.Audio,
			Value = "",
			Comment = $"Record {recordNumber}",
			Order = recordNumber
		};
		
		var fileName = GetFileName("audio.mp4", momento.Id.ToString());
		
		momento.Value = fileName;
		
		await fileStorageService.UploadPrivateFileAsync(file, fileName, FileStorageKeys.MomentoFiles);

		await momentoRepository.CreateAsync(momento);
		
		return $"{momento.Id}-comment-{momento.Comment}";
	}

	public async Task<List<AudioResponseDto>> GetAudios(Guid userId, Guid targetUserId, List<Guid> alreadyLoaded)
	{
		List<Momento> audios = await GetAudioFromDatabase(userId, targetUserId, alreadyLoaded);

		return audios.Select(x => new AudioResponseDto
		{
			FileName = x.Value,
			Comment = x.Comment ?? "",
			CreatedDate = x.CreatedAt,
		}).ToList();
	}

	public async Task<MemoryStream> GetAudioFiles(Guid userId, Guid targetUserId, List<Guid> alreadyLoaded)
	{
		List<Momento> audios = await GetAudioFromDatabase(userId, targetUserId, alreadyLoaded);
		
		return await fileStorageService.GetFilesAsync(
			audios.Select(x => x.Value).ToList(),
			FileStorageKeys.MomentoFiles);
	}
	
	public async Task Delete(Guid id)
	{
		var momento = await momentoRepository.GetByIdAsync(id);
		
		await Task.WhenAll(
			momentoRepository.DeleteAsync(id),
			fileStorageService.DeleteFileAsync(FileStorageKeys.MomentoFiles, momento.Value)
		);
	}

	public async Task UpdateComment(Guid id, string comment)
	{
		var momento = await momentoRepository.GetByIdAsync(id);
		
		momento.Comment = comment;
		
		await momentoRepository.UpdateAsync(momento);
	}
	
	private async Task<List<Momento>> GetAudioFromDatabase(Guid userId, Guid targetUserId, List<Guid> alreadyLoaded)
	{
		var audios = await momentoRepository.GetAsync(x => 
			x.UserId == userId 
			&& x.TargetUserId == targetUserId
			&& !alreadyLoaded.Contains(x.Id)
			&& x.Type == MomentoType.Audio
		);
		return audios;
	}
	
	private static string GetFileName(string name, params string[] ids)
	{
		string prefix = string.Join('-', ids);

		return $"{prefix}-{Path.GetFileName(name)}";
	}
}