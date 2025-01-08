﻿namespace ExpoApp.Domain.Entities.UserQrCodes;

public interface IUserQrCodeService
{
	Task<byte[]> GenerateQrCodeAsync(Guid userId);
}