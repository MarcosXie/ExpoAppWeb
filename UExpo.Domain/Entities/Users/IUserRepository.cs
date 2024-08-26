using AutoMapper;
using UExpo.Domain.Dao;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Entities.Users;

public interface IUserRepository : IBaseRepository<UserDao, User>
{
    Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task DeleteUserWithNotValidatedEmailsAsync(string email, CancellationToken cancellationToken = default);
    Task<int> GetImageMaxOrderByUserIdAsync(Guid id);
    Task<User> GetByIdDetailedAsync(Guid id);
    Task AddImagesAsync(List<UserImage> images);
    Task RemoveImagesAsync(List<UserImage> images);
}
