using ExpoApp.Domain.Entities.Momento;
using Microsoft.AspNetCore.Mvc;

namespace ExpoApp.Api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class MomentoController(IMomentoService momentoService) : ControllerBase
{
	[HttpPost("Audio")]
	public async Task<ActionResult> SaveAudio(IFormFile file)
	{
		if (file.Length == 0)
			return BadRequest("No file selected");
		
		var uri = await momentoService.AddAudio(file);
		
		return Ok(uri);
	}
	
	// [HttpGet("Audio/{id:guid}")]
	// public async Task<ActionResult> GetAudios(Guid id)
	// {
	// }
}
