namespace UExpo.Domain.Admins;

public interface IAdminService
{
    Task<string> CreateAsync(AdminDto admin);
    Task<string> LoginAsync(AdminLoginDto loginDto);
}
