using ExpoApp.Application.Services;
using ExpoApp.Domain.Entities.Exhibitors;
using Microsoft.Extensions.DependencyInjection;

namespace ExpoApp.Application.Extensions;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
	    services.AddScoped<IExhibitorService, ExhibitorService>();
    }
}
