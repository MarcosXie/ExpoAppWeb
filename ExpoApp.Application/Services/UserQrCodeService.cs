using ExpoApp.Domain.Entities.UserQrCodes;
using ExpoShared.Application.Utils;
using ExpoShared.Domain.Entities.Users;

namespace ExpoApp.Application.Services;

public class UserQrCodeService(IUserRepository userRepository) : IUserQrCodeService
{
	public Task<byte[]> GenerateQrCodeAsync(Guid userId)
	{
		var qrCode = QRCodeHelper.GenerateQRCode($"expoapp://add-relationship/{userId.ToString()}");
		
		return Task.FromResult(qrCode);
	}

	public async Task<byte[]> GenerateQrCodeByEmailAsync(string email)
	{
		var user = await userRepository.FirstOrDefaultAsync(x => x.Email == email);

		if (user == null)
		{
			return null;
		}
		
		var qrCode = await GenerateQrCodeAsync(user.Id);
		return qrCode;
	}
}