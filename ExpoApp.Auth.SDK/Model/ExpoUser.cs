// ExpoApp.Auth.SDK/Models/ExpoUser.cs
namespace ExpoApp.Auth.SDK.Model;

public class ExpoUser
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string Enterprise { get; set; } = string.Empty;
	public string Type { get; set; } = string.Empty;
	public string ProfileImageUri { get; set; } = string.Empty;
}