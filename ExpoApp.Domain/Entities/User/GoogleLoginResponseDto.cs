namespace ExpoApp.Domain.Entities.User;

public class GoogleLoginResponseDto
{
	public string? Hash { get; set; }
	public bool IsFirstTimeLogin { get; set; }
}
