// ExpoApp.Auth.SDK/Helpers/TokenParser.cs
using System.IdentityModel.Tokens.Jwt;
using ExpoApp.Auth.SDK.Model;

public static class TokenParser
{
	public static ExpoUser? ParseExpoToken(string token)
	{
		try
		{
			var handler = new JwtSecurityTokenHandler();
			var jwtToken = handler.ReadJwtToken(token);

			return new ExpoUser
			{
				Id = Guid.Parse(jwtToken.Claims.First(c => c.Type == "id").Value),
				Name = jwtToken.Claims.First(c => c.Type == "name").Value,
				Email = jwtToken.Claims.First(c => c.Type == "email").Value,
				Enterprise = jwtToken.Claims.FirstOrDefault(c => c.Type == "enterprise")?.Value ?? "",
				Type = jwtToken.Claims.First(c => c.Type == "type").Value,
				ProfileImageUri = jwtToken.Claims.FirstOrDefault(c => c.Type == "profileImageUri")?.Value ?? ""
			};
		}
		catch
		{
			// Se o token for inválido ou faltar claims obrigatórias
			return null;
		}
	}
}
