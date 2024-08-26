using Microsoft.AspNetCore.Mvc;
using UExpo.Domain.Entities.CallCenterChat;

namespace UExpo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CallCenterController(ICallCenterChatService service) : ControllerBase
{
    [HttpGet("Request")]
    public async Task<ActionResult<CallCenterChatResponseDto>> RequestCallCenterAsync()
    {
        CallCenterChatResponseDto callCenter = await service.GetChatByUserIdAsync();

        return Ok(callCenter);
    }
}
