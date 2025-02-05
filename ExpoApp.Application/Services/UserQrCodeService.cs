using ExpoApp.Domain.Entities.UserQrCodes;
using ExpoShared.Application.Utils;

namespace ExpoApp.Application.Services;

public class UserQrCodeService : IUserQrCodeService
{
	public Task<byte[]> GenerateQrCodeAsync(Guid userId)
	{
		var qrCode = QRCodeHelper.GenerateQRCode($"expoapp://add-relationship/{userId.ToString()}");
		
		return Task.FromResult(qrCode);
	}
}