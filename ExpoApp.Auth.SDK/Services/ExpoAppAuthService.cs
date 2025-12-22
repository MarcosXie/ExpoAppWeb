// ExpoApp.Auth.SDK/Services/IExpoAuthService.cs

using System.Net.Http.Json;

namespace ExpoApp.Auth.SDK.Services;

public interface IExpoAuthService
{
	Task<AuthResult?> LoginAndParseAsync(string email, string password);
}

// ExpoApp.Auth.SDK/Services/ExpoAuthService.cs
public class ExpoAuthService(HttpClient httpClient) : IExpoAuthService
{
	public async Task<AuthResult?> LoginAndParseAsync(string email, string password)
	{
		var response = await httpClient.PostAsJsonAsync("Api/User/Login", new { Email = email, Password = password });
    
		if (!response.IsSuccessStatusCode) return null;

		var token = await response.Content.ReadAsStringAsync();
		// Remove aspas extras se a API retornar a string pura
		token = token.Trim('"'); 

		return new AuthResult
		{
			Token = token,
			User = TokenParser.ParseExpoToken(token)
		};
	}
}
