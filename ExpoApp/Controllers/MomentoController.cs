using ExpoApp.Domain.Entities.Momento;
using Microsoft.AspNetCore.Mvc;

namespace ExpoApp.Api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class MomentoController(IMomentoService momentoService) : ControllerBase
{
	[HttpPost("Audio/{targetUserId}")]
	public async Task<ActionResult> SaveAudio(IFormFile file, Guid targetUserId)
	{
		if (file.Length == 0)
			return BadRequest("No file selected");
		
		var uri = await momentoService.AddAudio(file, targetUserId);
		
		return Ok(uri);
	}
	
	[HttpGet("Audio/{userId:guid}/{targetUserId:guid}")]
	public async Task<ActionResult> GetAudios(Guid userId, Guid targetUserId)
	{
		var audios = await momentoService.GetAudios(userId, targetUserId);
		
		return File(audios, "application/zip", "audios.zip");
	}
}
