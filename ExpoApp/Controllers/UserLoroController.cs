using ExpoShared.Domain.Entities.UserLoros;
using ExpoShared.Domain.Entities.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpoApp.Api.Controllers;

[ApiController]
[Route("Api/[controller]")]
public class UserLoroController(IUserLoroService service) : ControllerBase
{
    private readonly IUserLoroService service = service;
    
    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<string>> RegisterUserAsync(UserLoroDto user)
    {
        Guid id = await service.CreateUserAsync(user);
        return Ok(id);
    }
    
	[HttpPost("Login")]
    [AllowAnonymous]
    public async Task<ActionResult<string>> LoginAsync(LoroLoginDto loginDto)
    {
        string? hash = await service.LoginAsync(loginDto);

        return Ok(hash);
    }
	
	[HttpPost("UpdateFcmToken")]
	public async Task<ActionResult<UserProfileResponseDto>> UpdateFcmToken([FromBody] string token)
	{
		await service.UpdateFcmToken(token);
		
		return Ok();
	}
}
