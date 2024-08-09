using AutoMapper;
using Microsoft.Extensions.Configuration;
using UExpo.Application.Utils;
using UExpo.Domain.Admins;
using UExpo.Domain.Exceptions;

namespace UExpo.Application.Services.Admins;

public class AdminService : IAdminService
{
    private IAdminRepository _repository;
    private IMapper _mapper;
    private IConfiguration _config;

    public AdminService(IAdminRepository repository, IMapper mapper, IConfiguration config)
    {
        _repository = repository;
        _mapper = mapper;
        _config = config;
    }

    public async Task<string> CreateAsync(AdminDto admin)
    {
        var user = _mapper.Map<Admin>(admin);

        user.Password = HashHelper.Hash(admin.Password);

        return (await _repository.CreateAsync(user)).ToString();
    }

    public async Task<string> LoginAsync(AdminLoginDto loginDto)
    {
        var user = await _repository.GetByNameAsync(loginDto.Name)
                ?? throw new InvalidCredentialsException();

        if (!HashHelper.Verify(loginDto.Password, user.Password))
            throw new InvalidCredentialsException();

        return JwtHelper.GenerateJwtToken(user, _config);
    }
}
