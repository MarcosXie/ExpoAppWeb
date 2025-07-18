using ExpoShared.Domain.Exceptions;
using ExpoShared.Domain.SpeechTranslation;
using Microsoft.AspNetCore.Mvc;

namespace ExpoApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpeechController : ControllerBase
{
	private readonly ISpeechService _speechService;

	public SpeechController(ISpeechService speechService)
	{
		_speechService = speechService;
	}

	[HttpPost]
	public async Task<IActionResult> Translate([FromForm] TranslationRequestDto request)
	{
		if (request.AudioFile.Length == 0)
		{
			return BadRequest("Audio file is required.");
		}

		var result = await _speechService.TranslateAudioAsync(request);

		if (result == null)
		{
			throw new BadRequestException("Error communicating with azure.");
		}

		return Ok(result);
	}
}