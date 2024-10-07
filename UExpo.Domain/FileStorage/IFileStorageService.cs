using Microsoft.AspNetCore.Http;

namespace UExpo.Domain.FileStorage;

public interface IFileStorageService
{
    Task<string> UploadFileAsync(IFormFile file, string fileName, string bucket);
    Task<string> UploadFileAsync(string base64File, string fileName, string bucket);
	Task DeleteFileAsync(string bucket, string fileName);
}
