using Microsoft.AspNetCore.Mvc;
using UExpo.Domain.Users;

namespace UExpo.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IUserService service) : ControllerBase
{
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
}
