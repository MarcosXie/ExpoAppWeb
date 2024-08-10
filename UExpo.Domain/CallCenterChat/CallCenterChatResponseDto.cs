namespace UExpo.Domain.CallCenterChat;

public class CallCenterChatResponseDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = null!;
    public string UserCountry { get; set; } = null!;
    public string? UserEnterprise { get; set; }
    public DateTime? RegisterDate { get; set; }
    public int NotReadedMessages { get; set; }
}
