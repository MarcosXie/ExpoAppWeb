using ExpoApp.Domain.Entities.Calls;
using Microsoft.AspNetCore.Mvc;

namespace ExpoApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CallController(ICallService callService) : ControllerBase
{
	[HttpPost("initiate")]
	public async Task<ActionResult<InitiateCallResponseDto>> InitiateCall(InitiateCallDto dto)
	{
		var result = await callService.InitiateCallAsync(dto);
		return Ok(result);
	}

	[HttpPost("accept/{callId}")]
	public async Task<ActionResult<AcceptCallResponseDto>> AcceptCall(string callId)
	{
		var result = await callService.AcceptCallAsync(callId);
		return Ok(result);
	}

	[HttpPost("reject/{callId}")]
	public async Task<ActionResult> RejectCall(string callId)
	{
		await callService.RejectCallAsync(callId);
		return Ok();
	}

	[HttpPost("end/{callId}")]
	public async Task<ActionResult> EndCall(string callId, [FromQuery] string userId)
	{
		await callService.EndCallAsync(callId, userId);
		return Ok();
	}
}

