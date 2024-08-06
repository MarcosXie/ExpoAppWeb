using AutoMapper;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using UExpo.Application.Utils;
using UExpo.Domain.Users;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using UExpo.Domain.Email;
using UExpo.Domain.Exceptions;

namespace UExpo.Application.Services.Users;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;
    private readonly IConfiguration _config;
    private readonly IEmailService _emailService;

    public UserService(
        IUserRepository repository,
        IMapper mapper, 
        IConfiguration config, 
        IEmailService emailService)
    {
        _repository = repository;
        _mapper = mapper;
        _config = config;
        _emailService = emailService;
    }

    public async Task<Guid> CreateUserAsync(UserDto userDto)
    {
        await ValidateCreateAsync(userDto);

        var user = _mapper.Map<User>(userDto);

        user.Password = HashHelper.Hash(userDto.Password);

        await SendEmailConfirmationEmailAsync(user);

        return await _repository.CreateAsync(user);
    }

    public async Task<string?> LoginAsync(LoginDto loginDto)
    {
        var user = await _repository.GetUserByEmailAsync(loginDto.Email)
            ?? throw new InvalidCredentialsException();

        ValidateUserEmail(user);

        if (!HashHelper.Verify(loginDto.Password, user.Password))
            throw new InvalidCredentialsException();

        var token = GenerateJwtToken(user, loginDto.UserType);
        return token;
    }

    public async Task VerifyEmailAsync(Guid id, string code)
    {
        var user = await _repository.GetByIdAsync(id);

        var baseCode = GenerateBaseValidationCode(user);

        var isValid = HashHelper.Verify(baseCode, code);

        if (!isValid)
            throw new Exception("Error validating user email!");

        user.IsEmailValidated = true;

        await _repository.UpdateAsync(user);

        await _repository.DeleteUserWithNotValidatedEmailsAsync(user.Email);
    }

    public async Task ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
    {
        var user = await _repository.GetUserByEmailAsync(forgotPasswordDto.Email);

        if (user is null)
            throw new NotFoundException(nameof(user) + $"with email: {forgotPasswordDto.Email}");

        ValidateUserEmail(user);

        await SendEmailForgotPasswordAsync(user);
    }

    #region Utils
    private async Task ValidateCreateAsync(UserDto userDto)
    {
        if (!userDto.Password.Equals(userDto.ConfirmPassword))
            throw new BadRequestException("The passwords don`t match! Please try again!");

        var email = await _repository.GetUserByEmailAsync(userDto.Email);

        if (email is not null && email.IsEmailValidated)
            throw new BadRequestException("This email is already registered in UExpo!");
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

    private async Task SendEmailConfirmationEmailAsync(User user)
    {
        var code = HashHelper.Hash(GenerateBaseValidationCode(user));

        EmailSendDto emailSendDto = new()
        {
            ToAddresses = [user.Email],
            Subject = $"Welcome {user.Name} to UExpo! Please verify your email.",
            Body = @$"<p>Click in the link bellow to authenticate your email: 
                    <br>
                    <a href=""{_config["FrontEndUrl"]}/080_verify_email/{user.Id}/{code}"">Authenticate</a>
                    <br>
                    Thank you!</p>"
        };

        await _emailService.SendEmailAsync(emailSendDto);
    }

    private async Task SendEmailForgotPasswordAsync(User user)
    {
        //TODO: Modificar para retornar a senha verdadeira
        EmailSendDto emailSendDto = new()
        {
            ToAddresses = [user.Email],
            Subject = $"UExpo - Forgot password",
            Body = @$"<p>You requested your password from UExpo, if are not you please ignore this email.
                        <br>
                        Password:<strong>{user.Password}</strong>
                      </p>"
        };

        await _emailService.SendEmailAsync(emailSendDto);
    }

    private static string GenerateBaseValidationCode(User user) => 
        $"{user.Name}{user.Email}{user.Password}";
    private static void ValidateUserEmail(User user)
    {
        if (!user.IsEmailValidated)
            throw new BadRequestException("Email not validated!");
    }
    #endregion
}
