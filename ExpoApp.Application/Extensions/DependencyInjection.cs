using ExpoApp.Application.Services;
using ExpoApp.Domain.Entities.Exhibitors;
using ExpoApp.Domain.Entities.Momento;
using ExpoApp.Domain.Entities.UserQrCodes;
using ExpoApp.Domain.Entities.Wed;
using Microsoft.Extensions.DependencyInjection;

namespace ExpoApp.Application.Extensions;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
	    services.AddScoped<IExhibitorService, ExhibitorService>();
	    services.AddScoped<IUserQrCodeService, UserQrCodeService>();
	    services.AddScoped<IMomentoService, MomentoService>();
	    services.AddScoped<IPresentService, PresentService>();
    }
}
