namespace UExpo.Domain.Users;

public interface IUserService
{
    Task<Guid> CreateUserAsync(UserDto userDto);
    Task<string> LoginAsync(LoginDto loginDto);
}
