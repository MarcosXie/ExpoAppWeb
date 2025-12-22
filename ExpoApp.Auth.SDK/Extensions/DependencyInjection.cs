// ExpoApp.Auth.SDK/Extensions/ServiceCollectionExtensions.cs

using System.Text;
using ExpoApp.Auth.SDK.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ExpoApp.Auth.SDK;

public static class DependencyInjection
{
	public static IServiceCollection AddExpoBotAuth(this IServiceCollection services, IConfiguration config)
	{
		// 1. Configura o Cliente HTTP para a API Central
		services.AddHttpClient<IExpoAuthService, ExpoAuthService>(client =>
		{
			client.BaseAddress = new Uri("https://api.expoapp.com.br/");
		});

		// 2. Configura a Validação do JWT (Shared Key)
		services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidIssuer = "ExpoApp", // Deve ser o mesmo que o JwtHelper gera
					ValidateAudience = false,
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(
						Encoding.UTF8.GetBytes(config["Jwt:Key"] ?? throw new Exception("JWT Key missing")))
				};
			});

		return services;
	}
}