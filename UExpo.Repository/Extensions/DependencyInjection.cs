using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UExpo.Domain.Admins;
using UExpo.Domain.CallCenterChat;
using UExpo.Domain.Catalogs;
using UExpo.Domain.Catalogs.ItemImages;
using UExpo.Domain.Catalogs.Pdfs;
using UExpo.Domain.FairDates;
using UExpo.Domain.Places;
using UExpo.Domain.Users;
using UExpo.Repository.Context;
using UExpo.Repository.Mapper;
using UExpo.Repository.Repositories;

namespace UExpo.Repository.Extensions;

public static class DependencyInjection
{
    public static void AddRepository(this IServiceCollection services, IConfiguration config)
    {
        services.SetProfileBuilder(_ => new DaoMapper());
        services.SetProfileBuilder(_ => new DomainToDto());
        services.SetProfileBuilder(_ => new DtoToDomain());
        services.CreateMappers();

        services.AddDbContext<UExpoDbContext>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICallCenterChatRepository, CallCenterChatRepository>();
        services.AddScoped<IAdminRepository, AdminRepository>();

        services.AddScoped<ICatalogRepository, CatalogRepository>();
        services.AddScoped<ICatalogPdfRepository, CatalogPdfRepository>();
        services.AddScoped<ICatalogItemImageRepository, CatalogItemImageRepository>();

        services.AddScoped<IPlaceRepository, PlaceRepository>();
        services.AddScoped<IFairDateRepository, FairDateRepository>();
    }
}