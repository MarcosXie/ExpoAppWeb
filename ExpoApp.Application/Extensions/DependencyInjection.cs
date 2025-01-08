using ExpoApp.Application.Services;
using ExpoApp.Domain.Entities.Exhibitors;
using ExpoApp.Domain.Entities.UserQrCodes;
using ExpoShared.Application.Services.Relationships;
using ExpoShared.Domain.Entities.Relationships;
using Microsoft.Extensions.DependencyInjection;

namespace ExpoApp.Application.Extensions;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
	    services.AddScoped<IExhibitorService, ExhibitorService>();
	    services.AddScoped<IUserQrCodeService, UserQrCodeService>();
    }
}
