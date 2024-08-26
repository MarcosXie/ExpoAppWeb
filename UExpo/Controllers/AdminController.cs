using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UExpo.Domain.Entities.Admins;

namespace UExpo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AdminController(IAdminService service) : ControllerBase
{
    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<ActionResult<string>> LoginAdmin(AdminLoginDto admin)
    {
        string token = await service.LoginAsync(admin);
        return Ok(token);
    }

    [HttpPost]
    public async Task<ActionResult> CreateAdmin(AdminDto admin)
    {
        await service.CreateAsync(admin);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAdmin(Guid id, AdminDto admin)
    {
        await service.UpdateAsync(id, admin);
        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<List<AdminResponseDto>>> GetAdmins()
    {
        List<AdminResponseDto> admins = await service.GetAdminsAsync();

        return Ok(admins);
    }

    [HttpPut("{id}/SwitchStatus")]
    public async Task<ActionResult<List<AdminResponseDto>>> SwitchStatusAdmin(Guid id)
    {
        await service.SwitchStatusAsync(id);

        return Ok();
    }
}
