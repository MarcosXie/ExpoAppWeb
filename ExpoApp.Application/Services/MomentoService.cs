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
	public async Task<MomentoResponseDto> AddMomentoFile(IFormFile file, Guid targetUserId, MomentoType type)
	{
		var userId = authUserHelper.GetUser().Id;
		int momentoOrder = await GetMomentoOrder(targetUserId, type, userId);
		string fileName = string.Empty;
		
		var momento = new Momento
		{
			UserId = userId,
			TargetUserId = targetUserId,
			Type = type,
			Value = "",
			Comment = type == MomentoType.Audio ?  $"Record {momentoOrder}": "",
			Order = momentoOrder
		};

		if (momento.Type != MomentoType.Memo)
		{
			fileName = GetFileName(BuildFileNameSuffix(type), momento.Id.ToString());
				
			momento.Value = fileName;
			
			await fileStorageService.UploadPrivateFileAsync(file, fileName, FileStorageKeys.MomentoFiles);
		}
		
		await momentoRepository.CreateAsync(momento);
		
		var dbMomento = await momentoRepository.GetByIdAsync(momento.Id);

		return new()
		{
			Id = momento.Id,
			CreatedDate = dbMomento.CreatedAt,
			Comment = momento.Comment,
			Value = fileName,
			Order = momentoOrder
		};
	}

	public async Task<MomentoResponseDto> AddMomentoText(string value, Guid targetUserId, MomentoType type = MomentoType.Memo)
	{
		var userId = authUserHelper.GetUser().Id;
		int momentoOrder = await GetMomentoOrder(targetUserId, type, userId);
		
		var momento = new Momento
		{
			UserId = userId,
			TargetUserId = targetUserId,
			Type = type,
			Value = value,
			Order = momentoOrder
		};
		
		await momentoRepository.CreateAsync(momento);

		return new()
		{
			Id = momento.Id,
			CreatedDate = momento.CreatedAt,
			Comment = "",
			Value = value,
			Order = momentoOrder
		};
	}

	public async Task<List<MomentoResponseDto>> GetMomentos(Guid userId, Guid targetUserId, MomentoType type, List<Guid> alreadyLoaded)
	{
		List<Momento> audios = await GetMomentosFromDatabase(userId, targetUserId, type, alreadyLoaded);

		return audios.Select(x => new MomentoResponseDto
		{
			Id = x.Id,
			Value = x.Value,
			Comment = x.Comment ?? "",
			CreatedDate = x.CreatedAt,
			Order = x.Order,
		}).ToList();
	}

	public async Task<MemoryStream> GetMomentoFiles(Guid userId, Guid targetUserId, MomentoType type, List<Guid> alreadyLoaded)
	{
		List<Momento> momentos = await GetMomentosFromDatabase(userId, targetUserId, type, alreadyLoaded);
		
		return await fileStorageService.GetFilesAsync(
			momentos.Select(x => x.Value).ToList(),
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
	
	public async Task UpdateValue(Guid id, string value)
	{
		var momento = await momentoRepository.GetByIdAsync(id);
		
		momento.Value = value;
		
		await momentoRepository.UpdateAsync(momento);
	}
	
	private async Task<List<Momento>> GetMomentosFromDatabase(Guid userId, Guid targetUserId, MomentoType type, List<Guid> alreadyLoaded)
	{
		return await momentoRepository.GetAsync(x => 
			x.UserId == userId 
			&& x.TargetUserId == targetUserId
			&& !alreadyLoaded.Contains(x.Id)
			&& x.Type == type
		);
	}
	
	private static string GetFileName(string name, params string[] ids)
	{
		string prefix = string.Join('-', ids);

		return $"{prefix}-{Path.GetFileName(name)}";
	}
	
	private async Task<int> GetMomentoOrder(Guid targetUserId, MomentoType type, Guid userId)
	{
		var momentos = await momentoRepository.GetAsync(x => 
			x.UserId == userId 
			&& x.TargetUserId == targetUserId
			&& x.Type == type
		);

		var momentoOrder = momentos.Count != 0 ? momentos.Max(x => x.Order) + 1 : 1;
		return momentoOrder;
	}

	private static string BuildFileNameSuffix(MomentoType type)
	{
		return type switch
		{
			MomentoType.Audio => "audio.mp3",
			MomentoType.Memo => "",
			MomentoType.Image => "image.jpg",
			MomentoType.Video => "video.mp4",
			_ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
		};
	}
}