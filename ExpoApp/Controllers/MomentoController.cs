using ExpoApp.Domain.Entities.Momento;
using Microsoft.AspNetCore.Mvc;

namespace ExpoApp.Api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class MomentoController(IMomentoService momentoService) : ControllerBase
{
	[HttpPost("{momentoType}/{targetUserId:guid}")]
	public async Task<ActionResult> SaveAudio(IFormFile file, string momentoType, Guid targetUserId)
	{
		if (file.Length == 0)
			return BadRequest("No file selected");
		
		MomentoType type = (MomentoType)Enum.Parse(typeof(MomentoType), momentoType);
		
		var response = await momentoService.AddMomentoFile(file, targetUserId, type);
		
		return Ok(response);
	}

	[HttpGet("{momentoType}/Files/{userId:guid}/{targetUserId:guid}")]
	public async Task<ActionResult> GetAudioFiles(string momentoType, Guid userId, Guid targetUserId, [FromQuery] List<Guid>? alreadyLoaded)
	{
		alreadyLoaded ??= [];
		
		MomentoType type = (MomentoType)Enum.Parse(typeof(MomentoType), momentoType);
		
		var audios = await momentoService.GetMomentoFiles(userId, targetUserId, type, alreadyLoaded);

		return File(audios, "application/zip", "audios.zip");
	}
	
	[HttpGet("{momentoType}/{userId:guid}/{targetUserId:guid}")]
	public async Task<ActionResult> GetMomentos(string momentoType, Guid userId, Guid targetUserId, [FromQuery] List<Guid>? alreadyLoaded)
	{
		alreadyLoaded ??= [];
		MomentoType type = (MomentoType)Enum.Parse(typeof(MomentoType), momentoType);
		
		var audios = await momentoService.GetMomentos(userId, targetUserId, type, alreadyLoaded);

		return Ok(audios);
	}
	
	[HttpDelete("{id:guid}")]
	public async Task<ActionResult> DeleteMomento(Guid id)
	{
		await momentoService.Delete(id);

		return Ok();
	}
	
	[HttpPatch("{id:guid}/Comment")]
	public async Task<ActionResult> UpdateCommentMomento(Guid id, [FromBody]string comment)
	{
		await momentoService.UpdateComment(id, comment);

		return Ok();
	}
	
	[HttpPatch("{id:guid}")]
	public async Task<ActionResult> UpdateValueMomento(Guid id, [FromBody]string value)
	{
		await momentoService.UpdateValue(id, value);

		return Ok();
	}
}
