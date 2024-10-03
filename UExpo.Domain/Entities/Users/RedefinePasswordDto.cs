namespace UExpo.Domain.Entities.Users;

public class RedefinePasswordDto
{
	public string Password { get; set; } = null!;
	public string ConfirmPassword { get; set; } = null!;
	public string Code { get; set; } = null!;
}

