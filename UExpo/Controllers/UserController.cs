using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UExpo.Domain.Entities.Users;

namespace UExpo.Api.Controllers;

[ApiController]
[Route("Api/[controller]")]
public class UserController(IUserService service) : ControllerBase
{
    private readonly IUserService service = service;

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<string>> RegisterUserAsync(UserDto user)
    {
        Guid id = await service.CreateUserAsync(user);
        return Ok(id);
    }

	[HttpGet("BeMember")]
	[AllowAnonymous]
	public ActionResult<string> GetBeMemberInfoAsync()
	{
		BeMemberInfoDto info = service.GetBeMemberInfo();
		return Ok(info);
	}

	[HttpPost("Login")]
    [AllowAnonymous]
    public async Task<ActionResult<string>> LoginAsync(LoginDto loginDto)
    {
        string? hash = await service.LoginAsync(loginDto);

        return Ok(hash);
    }

    [HttpPost("Verify/{id}/{code}")]
    [AllowAnonymous]
    public async Task<ActionResult<string>> VerifyEmailAsync(Guid id, string code)
    {
        await service.VerifyEmailAsync(id, code);
        return Ok("Successfully validated email! Now you are able to login!");
    }

    [HttpPost("ForgotPassword")]
    [AllowAnonymous]
    public async Task<ActionResult> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
    {
        await service.ForgotPasswordAsync(forgotPasswordDto);
        return Ok();
    }

    [HttpPut("Profile/{id}")]
    public async Task<ActionResult> UpdateProfileAsync(Guid id, UserProfileDto profile)
    {
        await service.UpdateProfileAsync(id, profile);
        return Ok();
    }

    [HttpPost("Profile/Image/{id}")]
    public async Task<ActionResult> AddImagesAsync(Guid id, List<IFormFile> images)
    {
        await service.AddImagesAsync(id, images);
        return Ok();
    }

    [HttpPost("Profile/Image/Delete/{id}")]
    public async Task<ActionResult> DeleteImageAsync(Guid id, [FromBody] ImageDeleteDto url)
    {
        await service.RemoveImageByUrlAsync(id, url.Url);
        return Ok();
    }

    [HttpGet("Profile/{id}")]
    public async Task<ActionResult<UserProfileResponseDto>> GetProfileAsync(Guid id)
    {
        UserProfileResponseDto profile = await service.GetProfileAsync(id);
        return Ok(profile);
    }
}
