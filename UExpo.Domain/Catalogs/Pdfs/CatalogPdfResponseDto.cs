namespace UExpo.Domain.Catalogs.Pdfs;

public class CatalogPdfResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Uri { get; set; } = null!;
}
