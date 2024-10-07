using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mime;
using UExpo.Domain.Exceptions;
using UExpo.Domain.FileStorage;
using UExpo.Infrastructure.Utils;

namespace UExpo.Infrastructure.Services;

public class S3Service : IFileStorageService
{
    private readonly IConfiguration _config;
    private readonly IAmazonS3 _s3Client;

    public S3Service(IConfiguration config)
    {
        _config = config;

        _s3Client = new AmazonS3Client(
            AwsUtils.GetAwsCredentials(config),
            RegionEndpoint.GetBySystemName(_config["AWS:Region"])
        );
    }

    public async Task<string> UploadFileAsync(IFormFile file, string fileName, string bucket)
    {
        string? bucketName = _config[$"S3:{bucket}"];

        using MemoryStream memoryStream = new MemoryStream();

        file.CopyTo(memoryStream);

        TransferUtilityUploadRequest uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = memoryStream,
            BucketName = bucketName,
            Key = fileName,
            ContentType = file.ContentType,
            CannedACL = S3CannedACL.PublicRead,
        };

        TransferUtility fileTransferUtility = new TransferUtility(_s3Client);
        await fileTransferUtility.UploadAsync(uploadRequest);

        return $"https://{bucketName}.s3.amazonaws.com/{fileName}";
    }

	public async Task<string> UploadFileAsync(string base64File, string fileName, string bucket)
	{
		string? bucketName = _config[$"S3:{bucket}"];

		// Remover o prefixo data URL se houver (exemplo: "data:image/png;base64,")
		var base64Data = base64File.Contains(",") ? base64File.Split(',')[1] : base64File;
		var fileBytes = Convert.FromBase64String(base64Data);

		using MemoryStream memoryStream = new MemoryStream(fileBytes);

		TransferUtilityUploadRequest uploadRequest = new TransferUtilityUploadRequest
		{
			InputStream = memoryStream,
			BucketName = bucketName,
			Key = fileName,
			CannedACL = S3CannedACL.PublicRead,
		};

		TransferUtility fileTransferUtility = new TransferUtility(_s3Client);
		await fileTransferUtility.UploadAsync(uploadRequest);

		return $"https://{bucketName}.s3.amazonaws.com/{fileName}";
	}

	public async Task DeleteFileAsync(string bucket, string fileName)
    {
        DeleteObjectRequest deleteObjectRequest = new DeleteObjectRequest
        {
            BucketName = _config[$"S3:{bucket}"],
            Key = fileName
        };

        DeleteObjectResponse response = await _s3Client.DeleteObjectAsync(deleteObjectRequest);

        if (response.HttpStatusCode != HttpStatusCode.NoContent)
            throw new BadRequestException($"Failed to delete S3 file {fileName}");
    }
}
