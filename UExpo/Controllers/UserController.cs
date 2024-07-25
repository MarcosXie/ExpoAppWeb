using Microsoft.AspNetCore.Mvc;
using UExpo.Domain.Users;
namespace UExpo.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService service;

    public UserController(IUserService service)
    {
        this.service = service;
    }

    [HttpPost]
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
    public async Task<IActionResult> LoginAsync(LoginDto loginDto)
    {
        try
        {
            var hash = await service.LoginAsync(loginDto);
            return Ok(hash);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
