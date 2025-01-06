using ExpoApp.Domain.EmailTemplates;
using ExpoShared.Domain.EmailTemplates;
using Microsoft.Extensions.DependencyInjection;

namespace ExpoApp.Domain.Extensions;

public static class DependencyInjection
{
	public static void AddDomain(this IServiceCollection services)
	{
		services.AddScoped<IConfirmationEmailTemplate, ConfirmationEmailTemplate>();
		services.AddScoped<IForgotPasswordEmailTemplate, ForgotPasswordTemplate>();
	}
}