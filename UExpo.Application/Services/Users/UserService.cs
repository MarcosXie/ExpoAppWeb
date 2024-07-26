using AutoMapper;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using UExpo.Application.Utils;
using UExpo.Domain.Users;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace UExpo.Application.Services.Users;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;
    private readonly IConfiguration _config;

    public UserService(IUserRepository repository, IMapper mapper, IConfiguration config)
    {
        _repository = repository;
        _mapper = mapper;
        _config = config;
    }

    public async Task<Guid> CreateUserAsync(UserDto userDto)
    {
        await ValidateCreateAsync(userDto);

        var user = _mapper.Map<User>(userDto);

        user.Password = HashHelper.HashPassword(userDto.Password);

        return await _repository.CreateAsync(user);
    }

    public async Task<string?> LoginAsync(LoginDto loginDto)
    {
        var user = await _repository.GetUserByEmailAsync(loginDto.Email);

        if (user is not null && HashHelper.VerifyPassword(loginDto.Password, user.Password))
        {
            var token = GenerateJwtToken(user, loginDto.UserType);
            return token;
        }

        return null;
    }

    #region Utils
    private async Task ValidateCreateAsync(UserDto userDto)
    {
        if (!userDto.Password.Equals(userDto.ConfirmPassword))
            throw new Exception("The passwords don`t match! Please try again!");

        if (await _repository.GetUserByEmailAsync(userDto.Email) is not null)
            throw new Exception("This email is already registered in UExpo!");
    }

    private string GenerateJwtToken(User user, TypeEnum userType)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Name),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("Email", user.Email),
            new Claim("UserType", userType.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_config["Jwt:ExpiresInMinutes"])),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    #endregion
}
