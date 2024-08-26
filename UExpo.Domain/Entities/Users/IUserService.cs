using Microsoft.AspNetCore.Http;

namespace UExpo.Domain.Entities.Users;

public interface IUserService
{
    Task AddImagesAsync(Guid id, List<IFormFile> images);
    Task RemoveImageByUrlAsync(Guid id, string Url);
    Task<Guid> CreateUserAsync(UserDto userDto);
    Task ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
    Task<UserProfileResponseDto> GetProfileAsync(Guid id);
    Task<string?> LoginAsync(LoginDto loginDto);
    Task UpdateProfileAsync(Guid id, UserProfileDto profile);
    Task VerifyEmailAsync(Guid id, string code);
}
