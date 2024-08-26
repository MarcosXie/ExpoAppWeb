namespace UExpo.Domain.Entities.CallCenterChat;

public interface IChatUser
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Language { get; set; }
}
