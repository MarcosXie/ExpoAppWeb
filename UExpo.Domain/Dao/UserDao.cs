using System.ComponentModel.DataAnnotations.Schema;
using UExpo.Domain.Entities.Users;

namespace UExpo.Domain.Dao;

public class UserDao : BaseDao
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Enterprise { get; set; }
    public string Password { get; set; } = null!;
    public string Country { get; set; } = null!;
    public bool IsEmailValidated { get; set; }
    public string? Address { get; set; }
    public string? Description { get; set; }

    [NotMapped]
    public TypeEnum Type { get; set; }

    public CallCenterChatDao? CallCenterChat { get; set; }
    public CatalogDao? Catalog { get; set; }
    public List<UserImageDao> Images { get; set; } = [];
    public List<ExhibitorFairRegisterDao> FairRegisters { get; set; } = [];
    public List<RelationshipDao> BuyerRelationships { get; set; } = [];
    public List<RelationshipDao> SupplierRelationships { get; set; } = [];
}
