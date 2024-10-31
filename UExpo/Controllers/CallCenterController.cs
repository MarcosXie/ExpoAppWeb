using Microsoft.AspNetCore.Mvc;
using UExpo.Domain.Entities.Chats.CallCenterChat;

namespace UExpo.Api.Controllers;

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
