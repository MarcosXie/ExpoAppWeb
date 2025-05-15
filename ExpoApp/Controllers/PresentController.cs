using ExpoApp.Domain.Entities.Wed;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpoApp.Api.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class PresentController : ControllerBase
    {
        private readonly IPresentService _presentService;

        public PresentController(IPresentService presentService)
        {
            _presentService = presentService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PresentDto present)
        {
            try
            {
                await _presentService.Create(present);
                return CreatedAtAction(nameof(GetPresents), null, present);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar o presente: {ex.Message}");
            }
        }

        [HttpPut("{id}/buy")]
        public async Task<IActionResult> BuyPresent(Guid id, [FromBody] BuyPresentDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Os dados de compra não podem ser nulos.");
            }

            try
            {
                await _presentService.BuyPresentAsync(id, dto);
                return Ok("Presente marcado como comprado com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao marcar o presente como comprado: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPresents()
        {
            try
            {
                var presents = await _presentService.GetPresentsAsync();
                return Ok(presents);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar os presentes: {ex.Message}");
            }
        }
    }
}