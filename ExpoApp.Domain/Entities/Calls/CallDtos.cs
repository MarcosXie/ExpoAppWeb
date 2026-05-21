namespace ExpoApp.Domain.Entities.Calls;

public class InitiateCallDto
{
	public string CallerId { get; set; } = string.Empty;
	public string CallerName { get; set; } = string.Empty;
	public string TargetUserId { get; set; } = string.Empty;
	public string CallerLang { get; set; } = "en";
	public string TargetLang { get; set; } = "en";
}

public class InitiateCallResponseDto
{
	public string CallId { get; set; } = string.Empty;
	public string RoomUrl { get; set; } = string.Empty;
	public string Status { get; set; } = "ringing";
}

public class AcceptCallResponseDto
{
	public string RoomUrl { get; set; } = string.Empty;
	public string CallerLang { get; set; } = string.Empty;
	public string TargetLang { get; set; } = string.Empty;
}

public class ContactDto
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Country { get; set; } = string.Empty;
	public string? CellPhone { get; set; }
}
