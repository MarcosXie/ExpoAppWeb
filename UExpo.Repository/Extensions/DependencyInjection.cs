using Microsoft.Extensions.DependencyInjection;
using UExpo.Repository.Context;
using UExpo.Repository.Mapper;

namespace UExpo.Repository.Extensions;

public static class DependencyInjection
{
    public static void AddRepository(this IServiceCollection services)
    {
        // TODO: Add Connection string of MySQL
        services.AddDbContext<UExpoDbContext>();

        services.SetProfileBuilder(_ => new DaoMapper());
        services.SetProfileBuilder(_ => new DomainToDto());
        services.SetProfileBuilder(_ => new DtoToDomain());
    }
}