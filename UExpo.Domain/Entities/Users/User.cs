using UExpo.Domain.Entities.Catalogs;
using UExpo.Domain.Entities.Chats.Shared;
using UExpo.Domain.Entities.Exhibitors;
using UExpo.Domain.Entities.Relationships;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Entities.Users;

public class User : BaseModel, IChatUser
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Enterprise { get; set; }
    public string Password { get; set; } = null!;
    public string Country { get; set; } = null!;
    public bool IsEmailValidated { get; set; } = false;
    public string Lang { get; set; } = "en";
    public string? Address { get; set; }
    public string? Description { get; set; }
	public string? ProfileImageName { get; set; }
	public string? ProfileImageUri { get; set; }
	public DateTime BirthDate { get; set; }
	public UserType Type { get; set; }

	public List<UserImage> Images { get; set; } = [];
	public Catalog? Catalog { get; set; }
	public List<Relationship> BuyerRelationships { get; set; } = [];
	public List<Relationship> SupplierRelationships { get; set; } = [];
	public List<ExhibitorFairRegister> FairRegisters { get; set; } = [];
}

