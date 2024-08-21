using Microsoft.AspNetCore.Mvc;
using UExpo.Application.Utils;
using UExpo.Domain.Authentication;
using UExpo.Domain.Catalogs;
using UExpo.Domain.Catalogs.ItemImages;
using UExpo.Domain.Catalogs.Pdfs;
using UExpo.Domain.Shared;

namespace UExpo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CatalogController(ICatalogService service, AuthUserHelper userHelper) : ControllerBase
{

    [HttpPost]
    public async Task<ActionResult<CatalogResponseDto>> GetOrCreateAsync()
    {
        AuthenticatedUser user = userHelper.GetUser();

        CatalogResponseDto catalog = await service.GetOrCreateAsync(user.Id);

        return Ok(catalog);
    }

    [HttpPost("{id}/Pdf")]
    public async Task<ActionResult<CatalogPdfResponseDto>> CreatePdfAsync(IFormFile file, Guid id)
    {
        if (file is null || file.Length == 0)
            return BadRequest("No file uploaded");

        if (!Path.GetExtension(file.FileName).Equals(".pdf", StringComparison.CurrentCultureIgnoreCase))
            return BadRequest("Invalid file type");

        var pdf = await service.AddPdfAsync(new() { CatalogId = id, File = file });

        return Ok(pdf);
    }

    [HttpDelete("{id}/Pdf/{pdfId}")]
    public async Task<ActionResult> DeletePdfAsync(Guid id, Guid pdfId)
    {
        await service.DeletePdfAsync(id, pdfId);

        return Ok();
    }

    [HttpPost("{id}/Data")]
    public async Task<ActionResult<List<Dictionary<string, object>>>> AddCatalogData(Guid id, IFormFile file)
    {
        if (file is null || file.Length == 0)
            return BadRequest("No file uploaded");

        List<string> supportedTypes = [".xls", ".xlsx"];

        if (!supportedTypes.Contains(Path.GetExtension(file.FileName)))
            return BadRequest("Invalid file type");

        List<Dictionary<string, object>> parsedData = await service.AddCatalogDataAsync(id, file);

        return Ok(parsedData);
    }

    [HttpPost("{id}/Data/Validade")]
    public async Task<ActionResult<ValidationErrorResponseDto>> ValidateAddCatalogData(Guid id, IFormFile file)
    {
        if (file is null || file.Length == 0)
            return BadRequest("No file uploaded");

        List<string> supportedTypes = [".xls", ".xlsx"];

        if (!supportedTypes.Contains(Path.GetExtension(file.FileName)))
            return BadRequest("Invalid file type");

        ValidationErrorResponseDto res = await service.ValidadeAddCatalogDataAsync(id, file);

        return Ok(res);
    }

    [HttpPost("{id}/Product/{productId}/Image")]
    public async Task<ActionResult<List<CatalogItemImageResponseDto>>> AddCatalogImage(
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

        var createdImages = await service.AddImagesAsync(id, productId, images);

        return Ok(createdImages);
    }

    [HttpGet("{id}/Product/{productId}/Image")]
    public async Task<ActionResult<CatalogItemImageResponseDto>> GetItemImages(Guid id, string productId)
    {
        var images = await service.GetImagesByProductAsync(id, productId);

        return Ok(images);
    }

    [HttpDelete("{id}/Product/{productId}/Image/{imageId}")]
    public async Task<ActionResult<CatalogItemImageResponseDto>> DeleteItemImages(Guid id, string productId, Guid imageId)
    {
        await service.DeleteImageAsync(id, productId, imageId);

        return Ok();
    }
}
