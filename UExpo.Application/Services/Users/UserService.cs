using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using UExpo.Application.Utils;
using UExpo.Domain.Email;
using UExpo.Domain.Entities.Users;
using UExpo.Domain.Exceptions;
using UExpo.Domain.FileStorage;

namespace UExpo.Application.Services.Users;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;
    private readonly IConfiguration _config;
    private readonly IEmailService _emailService;
    private readonly IFileStorageService _fileStorageService;
    private readonly AuthUserHelper _authUserHelper;

    public UserService(
        IUserRepository repository,
        IMapper mapper,
        IConfiguration config,
        IFileStorageService fileStorageService,
        IEmailService emailService,
        AuthUserHelper authUserHelper)
    {
        _repository = repository;
        _mapper = mapper;
        _config = config;
        _emailService = emailService;
        _fileStorageService = fileStorageService;
        _authUserHelper = authUserHelper;
    }

    public async Task<Guid> CreateUserAsync(UserDto userDto)
    {
        await ValidateCreateAsync(userDto);

        User user = _mapper.Map<User>(userDto);

        user.Password = HashHelper.Hash(userDto.Password);

        await SendEmailConfirmationEmailAsync(user);

        return await _repository.CreateAsync(user);
    }

    public async Task<string?> LoginAsync(LoginDto loginDto)
    {
        User user = await _repository.GetUserByEmailAsync(loginDto.Email)
            ?? throw new InvalidCredentialsException();

        ValidateUserEmail(user);

        if (!HashHelper.Verify(loginDto.Password, user.Password))
            throw new InvalidCredentialsException();

        user.Type = loginDto.UserType;

        return JwtHelper.GenerateJwtToken(user, _config);
    }

    public async Task VerifyEmailAsync(Guid id, string code)
    {
        User user = await _repository.GetByIdAsync(id);

        string baseCode = GenerateBaseValidationCode(user);

        bool isValid = HashHelper.Verify(baseCode, code);

        if (!isValid)
            throw new Exception("Error validating user email!");

        user.IsEmailValidated = true;

        await _repository.UpdateAsync(user);

        await _repository.DeleteUserWithNotValidatedEmailsAsync(user.Email);
    }

    public async Task ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
    {
        User? user = await _repository.GetUserByEmailAsync(forgotPasswordDto.Email);

        if (user is null)
            throw new NotFoundException(nameof(user) + $"with email: {forgotPasswordDto.Email}");

        ValidateUserEmail(user);

        await SendEmailForgotPasswordAsync(user);
    }

    public async Task UpdateProfileAsync(Guid id, UserProfileDto profile)
    {
        var signedInUser = _authUserHelper.GetUser();

        if(signedInUser.Type == "Exhibitor")
        {
            if (string.IsNullOrEmpty(profile.Enterprise))
                throw new BadRequestException("Company is required!");

            if (string.IsNullOrEmpty(profile.Address))
                throw new BadRequestException("Address is required!");

            if (string.IsNullOrEmpty(profile.Description))
                throw new BadRequestException("Description is required!");
        }

        var user = await _repository.GetByIdDetailedAsync(id);
        _mapper.Map(profile, user);
        user.Password = user.Password.Equals(profile.Password) ? profile.Password : HashHelper.Hash(profile.Password);

        await _repository.UpdateAsync(user);
    }
    public async Task<UserProfileResponseDto> GetProfileAsync(Guid id)
    {
        var user = await _repository.GetByIdDetailedAsync(id);
        var mappedUser = _mapper.Map<UserProfileResponseDto>(user);
        return mappedUser;
    }

    public async Task AddImagesAsync(Guid id, List<IFormFile> images)
    {
        var user = await _repository.GetByIdDetailedAsync(id);

        await _repository.AddImagesAsync(await CreateNewImagesAsync(images, user));
    }

    public async Task RemoveImageByUrlAsync(Guid id, string Url)
    {
        var user = await _repository.GetByIdDetailedAsync(id);

        var imagesToDelete = user.Images.Where(x => x.Uri.Equals(Url)).ToList();

        await RemoveImagesAsync(imagesToDelete);

        await _repository.RemoveImagesAsync(imagesToDelete);
    }

	public BeMemberInfoDto GetBeMemberInfo()
	{
		var value = double.Parse(_config.GetSection("PaymentInfo:MemberPrice").Value!, CultureInfo.InvariantCulture);
		var descount = double.Parse(_config.GetSection("PaymentInfo:MemberDescount").Value!, CultureInfo.InvariantCulture);

		if (descount > 100) descount = 100;
		if (descount < 0) descount = 0;

		return new()
		{
			Value = value,
			Descount = descount,
			FinalValue = Math.Round(value * (1 - descount / 100), 2)
		};
	}

	#region Utils
	private async Task ValidateCreateAsync(UserDto userDto)
    {
        if (!userDto.Password.Equals(userDto.ConfirmPassword))
            throw new BadRequestException("The passwords don`t match! Please try again!");

        User? email = await _repository.GetUserByEmailAsync(userDto.Email);

        if (email is not null && email.IsEmailValidated)
            throw new BadRequestException("This email is already registered in UExpo!");
    }

    private async Task SendEmailConfirmationEmailAsync(User user)
    {
        string code = HashHelper.Hash(GenerateBaseValidationCode(user));

        EmailSendDto emailSendDto = new()
        {
            ToAddresses = [user.Email],
            Subject = $"Welcome {user.Name} to UExpo! Please verify your email.",
            Body = @$"<p>Click in the link bellow to authenticate your email: 
                    <br>
                    <a href=""{_config["FrontEndUrl"]}/025_verify_email/{user.Id}/{code}"">Authenticate</a>
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

    private static string GetFileName(string name, params string[] ids)
    {
        string prefix = string.Join('-', ids);

        return $"{prefix}-{Path.GetFileName(name)}";
    }

    private async Task<List<UserImage>> CreateNewImagesAsync(List<IFormFile> profileImages, User user)
    {
        List<UserImage> images = [];
        int order = await _repository.GetImageMaxOrderByUserIdAsync(user.Id) + 1;

        foreach (var image in profileImages)
        {
            var fileName = GetFileName(image.Name, user.Id.ToString(), order++.ToString());

            UserImage userImage = new()
            {
                UserId = user.Id,
                Order = order,
                FileName = fileName,
                Uri = await _fileStorageService.UploadFileAsync(image, fileName, FileStorageKeys.UserImages)
            };

            images.Add(userImage);
        }

        return images;
    }

    private async Task RemoveImagesAsync(List<UserImage> removedImages)
    {
        foreach(var image in removedImages)
        {
            await _fileStorageService.DeleteFileAsync(FileStorageKeys.UserImages, image.FileName);
        }
    }
	#endregion
}
