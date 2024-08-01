using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UExpo.Domain.Users;

namespace UExpo.Controllers;

[ApiController]
[Route("Api/[controller]")]
public class UserController(IUserService service) : ControllerBase
{
    private readonly IUserService service = service;

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterUserAsync(UserDto user)
    {
        var id = await service.CreateUserAsync(user);
        return Ok(id);
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginAsync(LoginDto loginDto)
    {
        var hash = await service.LoginAsync(loginDto);

        return Ok(hash);
    }

    [HttpPost("Verify/{id}/{code}")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyEmailAsync(Guid id, string code)
    {
        await service.VerifyEmailAsync(id, code);
        return Ok("Successfully validated email! Now you are able to login!");
    }

    [HttpPost("ForgotPassword")]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
    {
        await service.ForgotPasswordAsync(forgotPasswordDto);
        return Ok();
    }

    [HttpGet("Private")]
    public IActionResult PrivateEndpoint()
    {
        return Ok();
    }

}
