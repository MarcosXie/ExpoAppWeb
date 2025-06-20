using ExpoApp.Domain.Entities.User;
using ExpoApp.Domain.Entities.UserQrCodes;
using ExpoShared.Domain.Entities.Shared;
using ExpoShared.Domain.Entities.Users;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpoApp.Api.Controllers;

[ApiController]
[Route("Api/[controller]")]
public class UserController(IUserService service, IUserQrCodeService qrCodeService, IConfiguration config) : ControllerBase
{
    private readonly IUserService service = service;
    
    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<string>> RegisterUserAsync(UserDto user)
    {
        Guid id = await service.CreateUserAsync(user);
        return Ok(id);
    }

	[HttpGet("BeMember")]
	[AllowAnonymous]
	public ActionResult<string> GetBeMemberInfoAsync()
	{
		BeMemberInfoDto info = service.GetBeMemberInfo();
		return Ok(info);
	}

	[HttpPost("Login")]
    [AllowAnonymous]
    public async Task<ActionResult<string>> LoginAsync(LoginDto loginDto)
    {
        string? hash = await service.LoginAsync(loginDto);

        return Ok(hash);
    }

    [HttpPost("Verify/{id}/{code}")]
    [AllowAnonymous]
    public async Task<ActionResult<string>> VerifyEmailAsync(Guid id, string code)
    {
        await service.VerifyEmailAsync(id, code);
        return Ok("Successfully validated email! Now you are able to login!");
    }

    [HttpPost("ForgotPassword")]
    [AllowAnonymous]
    public async Task<ActionResult> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
    {
        await service.ForgotPasswordAsync(forgotPasswordDto);
        return Ok();
    }

	[HttpPost("RedefinePassword/{id}")]
	[AllowAnonymous]
	public async Task<ActionResult> RedefinePasswordAsync(Guid id, RedefinePasswordDto redefinePassword)
	{
		await service.RedefinePasswordAsync(id, redefinePassword);
		return Ok();
	}

	[HttpPut("Profile/{id}")]
    public async Task<ActionResult> UpdateProfileAsync(Guid id, UserProfileDto profile)
    {
        await service.UpdateProfileAsync(id, profile);
        return Ok();
    }

    [HttpPost("Profile/Image/{id}")]
    public async Task<ActionResult> AddImagesAsync(Guid id, List<IFormFile> images)
    {
        await service.AddImagesAsync(id, images);
        return Ok();
    }

    [HttpPost("Profile/Image/Delete/{id}")]
    public async Task<ActionResult> DeleteImagesAsync(Guid id, [FromBody] ImageDeleteDto url)
    {
        await service.RemoveImageByUrlAsync(id, url.Url);
        return Ok();
    }

	[HttpPost("Profile/ProfileImage/{id}")]
	public async Task<ActionResult> AddProfileImagesAsync(Guid id, IFormFile image)
	{
		var uri = await service.AddProfileImageAsync(id, image);
		return Ok(uri);
	}
	
	[HttpPost("Profile/ProfileImage/{id}/formatted")]
    public async Task<ActionResult> AddProfileImagesFormattedAsync(Guid id, IFormFile image)
    {
    	var uri = await service.AddProfileImageAsync(id, image);
    	return Ok(new
	    {
		    Uri = uri
	    });
    }

	[HttpDelete("Profile/ProfileImage/{id}")]
	public async Task<ActionResult> DeleteProfileImageAsync(Guid id)
	{
		await service.RemoveProfileImageAsync(id);
		return Ok();
	}

	[HttpGet("MenuUnlock")]
	public async Task<ActionResult<MenuUnlockDto>> GetMenuUnlockAsync()
	{
		MenuUnlockDto menu = await service.GetMenuUnlockAsync();
		return Ok(menu);
	}


	[HttpGet("Profile/{id}")]
    public async Task<ActionResult<UserProfileResponseDto>> GetProfileAsync(Guid id)
    {
        UserProfileResponseDto profile = await service.GetProfileAsync(id);
        return Ok(profile);
    }

	[HttpPatch("{id}/Language")]
	public async Task<ActionResult<UserProfileResponseDto>> UpdateLanguage(Guid id, UpdateLanguageDto updateDto)
	{
		await service.UpdateLanguageAsync(id, updateDto);
		return Ok();
	}

	[HttpGet("{id}/Language")]
	public async Task<ActionResult<UserProfileResponseDto>> GetLanguage(Guid id)
	{
		var lang = await service.GetLanguageAsync(id);
		return Ok(lang);
	}
	
	[HttpGet("{id}/QRCode")]
	public async Task<ActionResult<UserProfileResponseDto>> GetQrCode(Guid id)
	{
		var qrCode = await qrCodeService.GenerateQrCodeAsync(id);
		
		var base64String = Convert.ToBase64String(qrCode);
		return Ok($"data:image/png;base64,{base64String}");
	}
	
	[HttpPost("UpdateFcmToken")]
	public async Task<ActionResult<UserProfileResponseDto>> UpdateFcmToken([FromBody] string token)
	{
		await service.UpdateFcmToken(token);
		
		return Ok();
	}
	
	[HttpPost("Google")]
	[AllowAnonymous]
	public async Task<ActionResult<GoogleLoginResponseDto>> GoogleLoginAsync([FromBody] GoogleLoginDto googleLoginDto)
	{
	    try
	    {
		    bool isFirstTime = false;
	        // Verify Google ID token
	        var payload = await VerifyGoogleToken(googleLoginDto.IdToken);
	        if (payload is null)
	        {
	            return BadRequest("Invalid Google ID token");
	        }

	        // Find or create user
	        var user = await service.FindUserByEmailAsync(payload.Email);
	        if (user == null)
	        {
		        isFirstTime = true;
	            // Create new user with Google data
	            var createUser = new UserDto
	            {
	                Email = payload.Email,
	                Name = payload.Name ?? "Google User",
	                Password = "", // No password for Google users
	                Country = "", // Default or prompt later
	                BirthDate = DateTime.Now, // Default or prompt later
	                Type = UserType.Visitor, // Adjust based on your enum
					SourceType = SourceType.ANDROID,
					ProfileImageUri = payload.Picture,
					Enterprise = "",
					ConfirmPassword = ""
	            };
	            await service.CreateUserAsync(createUser, true);
	            user = await service.FindUserByEmailAsync(payload.Email);
	        }

	        // Generate session token (same as LoginAsync)
	        string? hash = await service.LoginWithGoogle(user!.Id);
	        if (hash == null)
	        {
	            return BadRequest("Failed to generate session token");
	        }

	        return Ok(new GoogleLoginResponseDto
	        {
		        Hash = hash,
		        IsFirstTimeLogin = isFirstTime,
	        });
	    }
	    catch (Exception ex)
	    {
	        return StatusCode(500, $"Google login failed: {ex.Message}");
	    }
	}

	private async Task<GoogleJsonWebSignature.Payload?> VerifyGoogleToken(string idToken)
	{
	    try
	    {
	        var settings = new GoogleJsonWebSignature.ValidationSettings
	        {
	            Audience = new[] { config.GetValue<string>("GoogleCloudConsole") ?? "" }
	        };
	        return await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
	    }
	    catch
	    {
	        return null;
	    }
	}
}
