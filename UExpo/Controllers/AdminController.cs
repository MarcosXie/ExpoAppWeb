using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UExpo.Domain.Admins;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UExpo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController(IAdminService service) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> CreateAdmin(AdminDto admin)
        {
            await service.CreateAsync(admin);
            return Ok();
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> LoginAdmin(AdminLoginDto admin)
        {
            var token = await service.LoginAsync(admin);
            return Ok(token);
        }

    }
}
