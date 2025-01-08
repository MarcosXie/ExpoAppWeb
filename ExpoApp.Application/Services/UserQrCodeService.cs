using ExpoApp.Domain.Entities.UserQrCodes;
using ExpoShared.Application.Utils;

namespace ExpoApp.Application.Services;

public class UserQrCodeService : IUserQrCodeService
{
	public Task<byte[]> GenerateQrCodeAsync(Guid userId)
	{
		// TODO: TROCAR
		// var qrCode = QRCodeHelper.GenerateQRCode(userId.ToString());
		var qrCode = QRCodeHelper.GenerateQRCode("http://google.com");
		
		return Task.FromResult(qrCode);
	}
}