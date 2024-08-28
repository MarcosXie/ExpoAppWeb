using Microsoft.AspNetCore.Mvc;
using UExpo.Domain.Entities.CallCenterChat;
using UExpo.Domain.Entities.Expo;

namespace UExpo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExpoController(IExpoService service) : ControllerBase
{
	[HttpGet]
	public async Task<ActionResult<CallCenterChatResponseDto>> GetExpoAsync()
	{
		ExpoResponseDto callCenter = await service.GetCurrentExpoAsync();

		return Ok(callCenter);
	}
}
