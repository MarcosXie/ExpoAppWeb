using Microsoft.Extensions.DependencyInjection;
using UExpo.Domain.Email;
using UExpo.Infrastructure.Services;

namespace UExpo.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IEmailService, EmailServiceAws>();
    }
}