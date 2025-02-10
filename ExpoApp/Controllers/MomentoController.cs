using System.IO.Compression;
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
	
	[HttpGet("Audio/{userId:guid}")]
	public async Task<ActionResult> GetAudios(Guid userId)
	{
		var audios = await momentoService.GetAudios(userId);
		
		using MemoryStream zipStream = new();
		var count = 1;
		using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
		{
			foreach (var stream in audios)
			{
				var entry = archive.CreateEntry($"{count}", CompressionLevel.Optimal);
				await using var entryStream = entry.Open();
				stream.Position = 0;
				await stream.CopyToAsync(entryStream);
				count++;
			}
		}
		
		zipStream.Position = 0;
		
		return Ok(File(zipStream, "application/zip", "audios.zip"));
	}
}
