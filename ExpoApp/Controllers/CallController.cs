using ExpoApp.Domain.Entities.Calls;
using ExpoShared.Application.Utils;
using ExpoShared.Domain.Entities.UserLoros;
using Microsoft.AspNetCore.Mvc;

namespace ExpoApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CallController(ICallService callService, IUserLoroRepository userLoroRepository, AuthUserHelper authUserHelper, ILogger<CallController> logger) : ControllerBase
{


	[HttpPost("initiate")]
	public async Task<ActionResult<InitiateCallResponseDto>> InitiateCall(InitiateCallDto dto)
	{
		try
		{
			logger.LogInformation("[CallController] InitiateCall called: CallerId={CallerId}, TargetUserId={TargetUserId}, CallerName={CallerName}, CallerLang={CallerLang}, TargetLang={TargetLang}",
				dto.CallerId, dto.TargetUserId, dto.CallerName, dto.CallerLang, dto.TargetLang);

			var result = await callService.InitiateCallAsync(dto);

			logger.LogInformation("[CallController] InitiateCall success: CallId={CallId}, RoomUrl={RoomUrl}",
				result.CallId, result.RoomUrl);

			return Ok(result);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "[CallController] InitiateCall FAILED: {Message}", ex.Message);
			return StatusCode(400, new { error = ex.Message });
		}
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

	[HttpGet("contacts")]
	public async Task<ActionResult<List<ContactDto>>> GetContacts()
	{
		var currentUser = authUserHelper.GetUser();
		var allUsers = await userLoroRepository.GetAsync(u => u.Id != currentUser.Id);
		var contacts = allUsers.Select(u => new ContactDto
		{
			Id = u.Id,
			Name = u.Name,
			Country = u.Country,
			CellPhone = u.CellPhone
		}).ToList();
		return Ok(contacts);
	}
}
