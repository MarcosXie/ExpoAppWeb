using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace ExpoApp.Repository.Extensions;

public delegate Profile ProfileBuilder(IServiceProvider provider);

public static class ProfilesInjections
{
    private static readonly List<ProfileBuilder> Profiles = new();

    public static void SetProfileBuilder(this IServiceCollection services, ProfileBuilder profileBuilder)
    {
        Profiles.Add(profileBuilder);
    }

    public static void CreateMappers(this IServiceCollection services)
    {
        services.AddSingleton(provider => new MapperConfiguration(cfg =>
            cfg.AddProfiles(Profiles.Select(builder => builder(provider)))).CreateMapper());
    }
}