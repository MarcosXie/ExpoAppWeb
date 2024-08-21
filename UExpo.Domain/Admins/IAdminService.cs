

namespace UExpo.Domain.Admins;

public interface IAdminService
{
    Task<string> CreateAsync(AdminDto admin);
    Task<List<AdminResponseDto>> GetAdminsAsync();
    Task<string> LoginAsync(AdminLoginDto loginDto);
    Task SwitchStatusAsync(Guid id);
    Task UpdateAsync(Guid id, AdminDto admin);
}
