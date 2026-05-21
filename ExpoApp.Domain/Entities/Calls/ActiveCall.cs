namespace ExpoApp.Domain.Entities.Calls;

public class ActiveCall
{
	public string CallId { get; set; } = Guid.NewGuid().ToString();
	public string CallerId { get; set; } = string.Empty;
	public string CallerName { get; set; } = string.Empty;
	public string TargetUserId { get; set; } = string.Empty;
	public string RoomUrl { get; set; } = string.Empty;
	public string RoomName { get; set; } = string.Empty;
	public CallStatus Status { get; set; } = CallStatus.Ringing;
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	public string CallerLang { get; set; } = "en";
	public string TargetLang { get; set; } = "en";
}

