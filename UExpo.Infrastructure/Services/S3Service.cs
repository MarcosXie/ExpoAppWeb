using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net;
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
        var bucketName = _config[$"S3:{bucket}"];

        using var memoryStream = new MemoryStream();

        file.CopyTo(memoryStream);

        var uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = memoryStream,
            BucketName = bucketName,
            Key = fileName,
            ContentType = file.ContentType,
            CannedACL = S3CannedACL.PublicRead,
        };

        var fileTransferUtility = new TransferUtility(_s3Client);
        await fileTransferUtility.UploadAsync(uploadRequest);

        return $"https://{bucketName}.s3.amazonaws.com/{fileName}";
    }

    public async Task DeleteFileAsync(string bucket, string fileName)
    {
        var deleteObjectRequest = new DeleteObjectRequest
        {
            BucketName = _config[$"S3:{bucket}"],
            Key = fileName
        };

        var response = await _s3Client.DeleteObjectAsync(deleteObjectRequest);

        if (response.HttpStatusCode != HttpStatusCode.NoContent)
            throw new BadRequestException($"Failed to delete S3 file {fileName}");
    }

}
