namespace UExpo.Repository.Dao;

public class CallCenterMessageDao : BaseDao
{
    public Guid ChatId { get; set; }
    public CallCenterChatDao CallCenterChat { get; set; } = null!;
    public Guid SenderId { get; set; }    
    public string SenderLang { get; set; } = null!;
    public string SendedMessage { get; set; } = null!;
    public string ReceiverLang { get; set; } = null!;
    public string TranslatedMessage {  get; set; } = null!;
    public bool Readed { get; set; }
}
