using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace UExpo.Domain.Catalogs.Pdfs;

public class CatalogPdfDto
{
    public IFormFile File { get; set; } = null!;

    [JsonIgnore]
    public Guid CatalogId { get; set; }
}
