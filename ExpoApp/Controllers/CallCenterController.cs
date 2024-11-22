using ExpoShared.Domain.Entities.Chats.CallCenterChat;
using Microsoft.AspNetCore.Mvc;

namespace ExpoApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CallCenterController(ICallCenterChatService service) : ControllerBase
{
    [HttpGet("Request")]
    public async Task<ActionResult<CallCenterChatResponseDto>> RequestCallCenterAsync(string? language)
    {
        CallCenterChatResponseDto callCenter = await service.GetChatByUserIdAsync(language);

        return Ok(callCenter);
    }
}
