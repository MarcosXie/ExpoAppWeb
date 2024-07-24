using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UExpo.Domain.Users;
using UExpo.Repository.Context;

namespace UExpo.Application.Services.Users;

public class UserService: IUserService
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
        var user = _mapper.Map<User>(userDto);

        return await _repository.CreateAsync(user);
    }
}
