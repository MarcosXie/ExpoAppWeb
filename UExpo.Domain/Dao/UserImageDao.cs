namespace UExpo.Domain.Dao;

public class UserImageDao : BaseDao
{
    public string FileName { get; set; } = null!;
    public string Uri { get; set; } = null!;
    public int Order { get; set; }
    public Guid UserId { get; set; }

    public UserDao User { get; set; } = null!;
}
