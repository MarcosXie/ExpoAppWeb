using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UExpo.Domain.Users;

namespace UExpo.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IUserService service) : ControllerBase
{
    private readonly IUserService service = service;

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterUserAsync(UserDto user)
    {
        try
        {
            var id = await service.CreateUserAsync(user);
            return Ok(id);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginAsync(LoginDto loginDto)
    {
        try
        {
            var hash = await service.LoginAsync(loginDto);

            if (hash is null)
            {
                return Unauthorized();
            }

            return Ok(hash);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("Private")]
    public IActionResult PrivateEndpoint()
    {
        return Ok();
    }

}
