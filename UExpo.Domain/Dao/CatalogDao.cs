namespace UExpo.Domain.Dao;

public class CatalogDao : BaseDao
{
    public Guid UserId { get; set; }
    public UserDao User { get; set; } = null!;
    public string? JsonTable { get; set; }

    public ICollection<CatalogItemImageDao> ItemImages { get; set; } = [];
    public ICollection<CatalogPdfDao> Pdfs { get; set; } = [];
}