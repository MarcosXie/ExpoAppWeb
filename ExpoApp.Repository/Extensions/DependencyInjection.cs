using ExpoShared.Domain.Entities.Admins;
using ExpoShared.Domain.Entities.Agendas;
using ExpoShared.Domain.Entities.Calendars;
using ExpoShared.Domain.Entities.Calendars.Fairs;
using ExpoShared.Domain.Entities.Calendars.Segments;
using ExpoShared.Domain.Entities.Carts;
using ExpoShared.Domain.Entities.Catalogs;
using ExpoShared.Domain.Entities.Catalogs.CatalogSegments;
using ExpoShared.Domain.Entities.Catalogs.ItemImages;
using ExpoShared.Domain.Entities.Catalogs.Pdfs;
using ExpoShared.Domain.Entities.Chats.CallCenterChat;
using ExpoShared.Domain.Entities.Exhibitors;
using ExpoShared.Domain.Entities.Expo;
using ExpoShared.Domain.Entities.Fairs;
using ExpoShared.Domain.Entities.Fairs.Segments;
using ExpoShared.Domain.Entities.Relationships;
using ExpoShared.Domain.Entities.Tutorial;
using ExpoShared.Domain.Entities.Users;
using ExpoShared.Repository.Context;
using ExpoShared.Repository.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExpoApp.Repository.Extensions;

public static class DependencyInjection
{
    public static void AddRepository(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<UExpoDbContext>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAdminRepository, AdminRepository>();

        services.AddScoped<ICallCenterChatRepository, CallCenterChatRepository>();
        services.AddScoped<ICallCenterMessageRepository, CallCenterMessageRepository>();

		services.AddScoped<ICatalogRepository, CatalogRepository>();
        services.AddScoped<ICatalogPdfRepository, CatalogPdfRepository>();
        services.AddScoped<ICatalogItemImageRepository, CatalogItemImageRepository>();
        services.AddScoped<ICatalogSegmentRepository, CatalogSegmentRepository>();

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
		services.AddScoped<IRelationshipMessageRepository, RelationshipMessageRepository>();

		services.AddScoped<ICartRepository, CartRepository>();
		services.AddScoped<ICartItemRepository, CartItemRepository>();
		services.AddScoped<ICartMessageRepository, CartMessageRepository>();
	}
}