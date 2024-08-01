using System.ComponentModel.DataAnnotations.Schema;
using UExpo.Domain.Users;

namespace UExpo.Repository.Dao;

public class UserDao : BaseDao
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Enterprise { get; set; }
    public string Password { get; set; } = null!;
    public string Country { get; set; } = null!;
    public bool IsEmailValidated { get; set; }
    [NotMapped]
    public TypeEnum Type { get; set; }

    public CallCenterChatDao CallCenterChatDao { get; set; } = new();
}
