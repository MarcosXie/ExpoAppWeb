using ExpoApp.Auth.SDK.Model;

public class AuthResult
{
	public string Token { get; set; } = string.Empty;
	public ExpoUser? User { get; set; }
}