using AutoMapper;
using UExpo.Application.Utils;
using UExpo.Domain.Users;

namespace UExpo.Application.Services.Users;

public class UserService : IUserService
{
    private IUserRepository _repository;
    private IMapper _mapper;

    public UserService(IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Guid> CreateUserAsync(UserDto userDto)
    {
        if (!userDto.Password.Equals(userDto.ConfirmPassword))
            throw new Exception("The passwords don`t match! Please try again!");

        var user = _mapper.Map<User>(userDto);

        user.Password = HashHelper.HashPassword(userDto.Password);

        return await _repository.CreateAsync(user);
    }

    public Task<string> LoginAsync(LoginDto loginDto)
    {
        throw new NotImplementedException();
    }
}
