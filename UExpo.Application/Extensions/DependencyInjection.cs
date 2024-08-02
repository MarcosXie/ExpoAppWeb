using Microsoft.Extensions.DependencyInjection;
using UExpo.Application.Services.CallCenterChats;
using UExpo.Application.Services.Users;
using UExpo.Domain.CallCenterChat;
using UExpo.Domain.Users;

namespace UExpo.Application.Extensions;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICallCenterChatService, CallCenterChatService>();
    }
}
