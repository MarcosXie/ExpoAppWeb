using System.ComponentModel.DataAnnotations;

namespace ExpoApp.Domain.Entities.User;

public class GoogleLoginDto
{
	[Required]
	public string IdToken { get; set; } = null!;
}
