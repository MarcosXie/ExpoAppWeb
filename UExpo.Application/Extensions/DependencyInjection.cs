using Microsoft.Extensions.DependencyInjection;
using UExpo.Application.Services.Admins;
using UExpo.Application.Services.CallCenterChats;
using UExpo.Application.Services.Users;
using UExpo.Application.Utils;
using UExpo.Domain.Admins;
using UExpo.Domain.CallCenterChat;
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
    }
}
