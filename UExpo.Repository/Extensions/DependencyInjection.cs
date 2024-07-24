using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UExpo.Domain.Users;
using UExpo.Repository.Context;
using UExpo.Repository.Mapper;
using UExpo.Repository.Repositories;

namespace UExpo.Repository.Extensions;

public static class DependencyInjection
{
    public static void AddRepository(this IServiceCollection services)
    {
        services.SetProfileBuilder(_ => new DaoMapper());
        services.SetProfileBuilder(_ => new DomainToDto());
        services.SetProfileBuilder(_ => new DtoToDomain());
        services.CreateMappers();

        // TODO: Add Connection string of MySQL
        services.AddDbContext<UExpoDbContext>();

        services.AddScoped<IUserRepository, UserRepository>();
    }
}