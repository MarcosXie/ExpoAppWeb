namespace UExpo.Domain.Dao;

public interface IBaseDao
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
