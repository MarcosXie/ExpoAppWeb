using Microsoft.Extensions.DependencyInjection;
using UExpo.Application.Services.Admins;
using UExpo.Application.Services.Agendas;
using UExpo.Application.Services.Calendars;
using UExpo.Application.Services.CallCenterChats;
using UExpo.Application.Services.Catalogs;
using UExpo.Application.Services.Fairs;
using UExpo.Application.Services.Users;
using UExpo.Application.Utils;
using UExpo.Domain.Admins;
using UExpo.Domain.Agendas;
using UExpo.Domain.Calendar;
using UExpo.Domain.CallCenterChat;
using UExpo.Domain.Catalogs;
using UExpo.Domain.Fairs;
using UExpo.Domain.Fairs.Segments;
using UExpo.Domain.Users;

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

        //services.AddHostedService<YearlyTaskService>();
    }
}
