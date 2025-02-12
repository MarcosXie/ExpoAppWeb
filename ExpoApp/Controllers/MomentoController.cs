using ExpoApp.Domain.Entities.Momento;
using Microsoft.AspNetCore.Mvc;

namespace ExpoApp.Api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class MomentoController(IMomentoService momentoService) : ControllerBase
{
	[HttpPost("Audio/{targetUserId:guid}")]
	public async Task<ActionResult> SaveAudio(IFormFile file, Guid targetUserId)
	{
		if (file.Length == 0)
			return BadRequest("No file selected");
		
		var uri = await momentoService.AddAudio(file, targetUserId);
		
		return Ok(uri);
	}
	
	[HttpGet("Audio/Files/{userId:guid}/{targetUserId:guid}")]
	public async Task<ActionResult> GetAudioFiles(Guid userId, Guid targetUserId, [FromQuery] List<Guid>? alreadyLoaded)
	{
		alreadyLoaded ??= [];
		
		var audios = await momentoService.GetAudioFiles(userId, targetUserId, alreadyLoaded);

		return File(audios, "application/zip", "audios.zip");
	}
	
	[HttpGet("Audio/{userId:guid}/{targetUserId:guid}")]
	public async Task<ActionResult> GetAudios(Guid userId, Guid targetUserId, [FromQuery] List<Guid>? alreadyLoaded)
	{
		alreadyLoaded ??= [];
		
		var audios = await momentoService.GetAudios(userId, targetUserId, alreadyLoaded);

		return Ok(audios);
	}
	
	[HttpDelete("Audio/{id:guid}")]
	public async Task<ActionResult> DeleteAudio(Guid id)
	{
		await momentoService.Delete(id);

		return Ok();
	}
	
	[HttpPatch("Audio/{id:guid}")]
	public async Task<ActionResult> DeleteAudio(Guid id, string comment)
	{
		await momentoService.UpdateComment(id, comment);

		return Ok();
	}
}
