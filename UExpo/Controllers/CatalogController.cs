using Microsoft.AspNetCore.Mvc;
using UExpo.Application.Utils;
using UExpo.Domain.Catalogs;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UExpo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CatalogController(ICatalogService service, AuthUserHelper userHelper) : ControllerBase
{

    [HttpPost]
    public async Task<ActionResult<CatalogResponseDto>> GetOrCreateAsync()
    {
        Domain.Authentication.AuthenticatedUser user = userHelper.GetUser();

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

        Guid pdfId = await service.AddPdfAsync(new() { CatalogId = id, File = file });

        return Ok(pdfId);
    }

    [HttpDelete("{id}/Pdf/{pdfId}")]
    public async Task<ActionResult<CatalogResponseDto>> DeletePdfAsync(Guid id, Guid pdfId)
    {
        await service.DeletePdfAsync(id, pdfId);

        return Ok();
    }

    [HttpPost("{id}/Data")]
    public async Task<ActionResult<List<Dictionary<string, object>>>> AddCatalogData(Guid id, IFormFile data)
    {
        if (data is null || data.Length == 0)
            return BadRequest("No file uploaded");

        List<string> supportedTypes = [".xls", ".xlsx"];

        if (!supportedTypes.Contains(Path.GetExtension(data.FileName)))
            return BadRequest("Invalid file type");

        List<Dictionary<string, object>> parsedData = await service.AddCatalogDataAsync(id, data);

        return Ok(parsedData);
    }

    [HttpPost("{id}/Product/{productId}/Image")]
    public async Task<ActionResult<List<Dictionary<string, object>>>> AddCatalogData(
        Guid id, string productId, List<IFormFile> images)
    {
        if (images is null || images.Count == 0)
            return BadRequest("No file uploaded");

        List<string> supportedTypes =
        [
            // Imagens
            ".png",
            ".jpeg",
            ".jpg",
            ".gif",
            ".bmp",
            ".tiff",
            ".svg",
            ".webp",
            ".ico",
            ".heic",
            ".heif",

            // Vídeos
            ".mp4",
            ".avi",
            ".mkv",
            ".mov",
            ".wmv",
            ".flv",
            ".webm",
            ".m4v",
            ".mpg",
            ".mpeg",
            ".3gp",
            ".ogg"
        ];

        if (images.Any(img => !supportedTypes.Contains(Path.GetExtension(img.FileName))))
            return BadRequest("Invalid file type");

        await service.AddImagesAsync(id, productId, images);

        return Ok();
    }
}
