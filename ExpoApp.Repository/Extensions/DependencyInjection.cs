using ExpoApp.Domain.Entities.Momento;
using ExpoApp.Repository.Context;
using ExpoApp.Repository.Mapper;
using ExpoApp.Repository.Repositories;
using ExpoShared.Domain.Entities.Relationships;
using ExpoShared.Domain.Entities.Users;
using ExpoShared.Repository.Mapper;
using Microsoft.Extensions.DependencyInjection;

namespace ExpoApp.Repository.Extensions;

public static class DependencyInjection
{
    public static void AddRepository(this IServiceCollection services)
    {
	    services.SetProfileBuilder(_ => new DaoMapper());
	    services.SetProfileBuilder(_ => new DomainToDto());
	    services.SetProfileBuilder(_ => new DtoToDomain());
	    services.SetProfileBuilder(_ => new DomainToDomain());
	    services.SetProfileBuilder(_ => new ExpoAppDaoMapper());
	    services.SetProfileBuilder(_ => new ExpoAppDomainToDto());
	    services.SetProfileBuilder(_ => new ExpoAppDtoToDomain());
	    services.SetProfileBuilder(_ => new ExpoAppDomainToDomain());
	    services.CreateMappers();
	    
	    services.AddDbContext<ExpoAppDbContext>();

        services.AddScoped<IUserRepository, ExpoAppUserRepository>();
        services.AddScoped<IRelationshipRepository, ExpoAppRelationshipRepository>();
        services.AddScoped<IMomentoRepository, MomentoRepository>();
	}
}