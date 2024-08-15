using Microsoft.AspNetCore.Mvc;
using UExpo.Application.Utils;
using UExpo.Domain.Catalogs;

namespace UExpo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CatalogController(ICatalogService service, AuthUserHelper userHelper) : ControllerBase
{

    [HttpPost]
    public async Task<ActionResult<CatalogResponseDto>> GetOrCreateAsync()
    {
        var user = userHelper.GetUser();

        CatalogResponseDto catalog = await service.GetOrCreateAsync(user.Id);

        return Ok(catalog);
    }

    [HttpPost("{id}/Pdf")]
    public async Task<ActionResult<CatalogResponseDto>> CreatePdfAsync(IFormFile file, Guid id)
    {
        if (file is null || file.Length == 0)
            return BadRequest("No file uploaded");

        if (!Path.GetExtension(file.FileName).Equals(".pdf", StringComparison.CurrentCultureIgnoreCase))
            return BadRequest("Invalid file type");

        Guid pdfId = await service.AddPdfAsync(new() { CatalogId = id, File = file } );

        return Ok(pdfId);
    }

    [HttpDelete("{id}/Pdf/{pdfId}")]
    public async Task<ActionResult<CatalogResponseDto>> DeletePdfAsync(Guid id, Guid pdfId)
    {
        await service.DeletePdfAsync(id, pdfId);

        return Ok();
    }
}
