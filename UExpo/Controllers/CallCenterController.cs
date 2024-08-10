using Microsoft.AspNetCore.Mvc;
using UExpo.Domain.CallCenterChat;

namespace UExpo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CallCenterController(ICallCenterChatService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<CallCenterChatResponseDto>>> GetChats()
    {
        var chats = await service.GetChatsAsync();

        return Ok(chats);
    }
}
