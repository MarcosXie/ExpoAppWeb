namespace UExpo.Domain.Entities.Users;

public interface IUserService
{
    Task<Guid> CreateUserAsync(UserDto userDto);
    Task ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
    Task<UserProfileResponseDto> GetProfileAsync(Guid id);
    Task<string?> LoginAsync(LoginDto loginDto);
    Task UpdateProfileAsync(Guid id, UserProfileDto profile);
    Task VerifyEmailAsync(Guid id, string code);
}
