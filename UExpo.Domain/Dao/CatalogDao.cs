using UExpo.Domain.Dao.Shared;

namespace UExpo.Domain.Dao;

public class CatalogDao : BaseDao
{
    public Guid UserId { get; set; }
    public UserDao User { get; set; } = null!;
    public string? JsonTable { get; set; }
    public string Tags { get; set; } = string.Empty;

    public ICollection<CatalogItemImageDao> ItemImages { get; set; } = [];
    public ICollection<CatalogPdfDao> Pdfs { get; set; } = [];
    public ICollection<CatalogSegmentDao> Segments { get; set; } = [];
}