using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UExpo.Domain.Entities.Admins;
using UExpo.Domain.Entities.Agendas;
using UExpo.Domain.Entities.Calendars;
using UExpo.Domain.Entities.Calendars.Fairs;
using UExpo.Domain.Entities.Calendars.Segments;
using UExpo.Domain.Entities.Catalogs;
using UExpo.Domain.Entities.Catalogs.ItemImages;
using UExpo.Domain.Entities.Catalogs.Pdfs;
using UExpo.Domain.Entities.Chats.CallCenterChat;
using UExpo.Domain.Entities.Exhibitors;
using UExpo.Domain.Entities.Expo;
using UExpo.Domain.Entities.Fairs;
using UExpo.Domain.Entities.Fairs.Segments;
using UExpo.Domain.Entities.Relationships;
using UExpo.Domain.Entities.Tutorial;
using UExpo.Domain.Entities.Users;
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
        services.SetProfileBuilder(_ => new DomainToDomain());
        services.CreateMappers();

        services.AddDbContext<UExpoDbContext>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAdminRepository, AdminRepository>();

        services.AddScoped<ICallCenterChatRepository, CallCenterChatRepository>();

        services.AddScoped<ICatalogRepository, CatalogRepository>();
        services.AddScoped<ICatalogPdfRepository, CatalogPdfRepository>();
        services.AddScoped<ICatalogItemImageRepository, CatalogItemImageRepository>();

        services.AddScoped<IAgendaRepository, AgendaRepository>();
        services.AddScoped<IFairRepository, FairRepository>();
        services.AddScoped<ISegmentRepository, SegmentRepository>();

        services.AddScoped<ICalendarRepository, CalendarRepository>();
        services.AddScoped<ICalendarFairRepository, CalendarFairRepository>();
        services.AddScoped<ICalendarSegmentRepository, CalendarSegmentRepository>();
        services.AddScoped<IExhibitorFairRegisterRepository, ExhibitorFairRegisterRepository>();
        services.AddScoped<ILastSearchedTagsRepository, LastSearchedTagsRepository>();

		services.AddScoped<ITutorialRepository, TutorialRepository>();
		services.AddScoped<IRelationshipRepository, RelationshipRepository>();
	}
}