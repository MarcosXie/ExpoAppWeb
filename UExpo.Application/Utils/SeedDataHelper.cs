using Microsoft.Extensions.DependencyInjection;
using UExpo.Domain.Entities.Admins;

namespace UExpo.Application.Utils;

public static class SeedDataHelper
{
    private static readonly Guid _baseAdminGuid = new("ab6741fb-cfcc-42ea-a4a3-7b5e9cdd6506");

    public static void BootstrapAdmin(IServiceProvider serviceProvider)
    {
        using IServiceScope scope = serviceProvider.CreateScope();

        IAdminRepository repository = scope.ServiceProvider.GetRequiredService<IAdminRepository>();

        Admin? admin = repository.GetByIdOrDefaultAsync(_baseAdminGuid).Result;

        if (admin is not null)
            return;

        Admin baseAdmin = new()
        {
            Id = _baseAdminGuid,
            Name = "Marcos",
            Password = HashHelper.Hash("1"),
            Type = AdminType.Admin,
        };

        repository.CreateAsync(baseAdmin).Wait();
    }
}
