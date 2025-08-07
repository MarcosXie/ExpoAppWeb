using ExpoApp.Domain.Entities.User;
using ExpoApp.Domain.Entities.UserQrCodes;
using ExpoShared.Domain.Entities.Shared;
using ExpoShared.Domain.Entities.UserLoros;
using ExpoShared.Domain.Entities.Users;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpoApp.Api.Controllers;

[ApiController]
[Route("Api/[controller]")]
public class UserLoroController(IUserLoroService service, IConfiguration config) : ControllerBase
{
    private readonly IUserLoroService service = service;
    
    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<string>> RegisterUserAsync(UserLoroDto user)
    {
        Guid id = await service.CreateUserAsync(user);
        return Ok(id);
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
	            var createUser = new UserLoroDto
	            {
	                Email = payload.Email,
	                Name = payload.Name ?? "Google User",
	                Password = "", // No password for Google users
	                Country = "", // Default or prompt later
					SourceType = SourceType.ANDROID,
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
