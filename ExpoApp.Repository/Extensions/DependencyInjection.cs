using ExpoApp.Repository.Repositories;
using ExpoShared.Domain.Entities.Relationships;
using ExpoShared.Domain.Entities.Users;
using ExpoShared.Repository.Context;
using Microsoft.Extensions.DependencyInjection;

namespace ExpoApp.Repository.Extensions;

public static class DependencyInjection
{
    public static void AddRepository(this IServiceCollection services)
    {
        services.AddDbContext<UExpoDbContext>();

        services.AddScoped<IUserRepository, ExpoAppUserRepository>();
        services.AddScoped<IRelationshipRepository, ExpoAppRelationshipRepository>();
	}
}