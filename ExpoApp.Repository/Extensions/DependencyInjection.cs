using ExpoApp.Domain.Entities.Momento;
using ExpoApp.Repository.Context;
using ExpoApp.Repository.Repositories;
using ExpoShared.Domain.Entities.Relationships;
using ExpoShared.Domain.Entities.Users;
using Microsoft.Extensions.DependencyInjection;

namespace ExpoApp.Repository.Extensions;

public static class DependencyInjection
{
    public static void AddRepository(this IServiceCollection services)
    {
	    services.AddDbContext<ExpoAppDbContext>();

        services.AddScoped<IUserRepository, ExpoAppUserRepository>();
        services.AddScoped<IRelationshipRepository, ExpoAppRelationshipRepository>();
        services.AddScoped<IMomentoRepository, MomentoRepository>();
	}
}