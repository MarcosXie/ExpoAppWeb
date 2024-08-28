using Microsoft.Extensions.DependencyInjection;
using UExpo.Application.Services.Admins;
using UExpo.Application.Services.Agendas;
using UExpo.Application.Services.CalendarFairs;
using UExpo.Application.Services.Calendars;
using UExpo.Application.Services.CallCenterChats;
using UExpo.Application.Services.Catalogs;
using UExpo.Application.Services.Expos;
using UExpo.Application.Services.Fairs;
using UExpo.Application.Services.Tutorials;
using UExpo.Application.Services.Users;
using UExpo.Application.Utils;
using UExpo.Domain.Entities.Admins;
using UExpo.Domain.Entities.Agendas;
using UExpo.Domain.Entities.Calendar;
using UExpo.Domain.Entities.Calendar.Fairs;
using UExpo.Domain.Entities.CallCenterChat;
using UExpo.Domain.Entities.Catalogs;
using UExpo.Domain.Entities.Expo;
using UExpo.Domain.Entities.Fairs;
using UExpo.Domain.Entities.Fairs.Segments;
using UExpo.Domain.Entities.Tutorial;
using UExpo.Domain.Entities.Users;

namespace UExpo.Application.Extensions;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<AuthUserHelper>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICallCenterChatService, CallCenterChatService>();
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<ICatalogService, CatalogService>();
        services.AddScoped<IAgendaService, AgendaService>();
        services.AddScoped<IFairService, FairService>();
        services.AddScoped<ISegmentService, SegmentService>();
        services.AddScoped<ICalendarService, CalendarService>();
        services.AddScoped<ICalendarFairService, CalendarFairService>();
        services.AddScoped<ITutorialService, TutorialService>();
        services.AddScoped<IExpoService, ExpoService>();

		//services.AddHostedService<YearlyTaskService>();
	}
}
