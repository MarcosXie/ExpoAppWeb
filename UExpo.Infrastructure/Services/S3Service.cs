using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System.Diagnostics;
using System.Net;
using UExpo.Domain.Exceptions;
using UExpo.Domain.FileStorage;
using UExpo.Infrastructure.Utils;

namespace UExpo.Infrastructure.Services;

public class S3Service : IFileStorageService
{
    private readonly IConfiguration _config;
    private readonly IAmazonS3 _s3Client;
	private readonly List<string> _imageExtensions = ["jpg", "jpeg", "png", "gif", "bmp", "webp"];
	private readonly List<string> _videoExtensions = ["mp4", "mov", "avi", "mkv", "webm"];


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

		using MemoryStream memoryStream = await GetMemoryStream(fileBytes, fileName);

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

	private async Task<MemoryStream> GetMemoryStream(byte[] fileBytes, string fileName)
	{
		if (_imageExtensions.Any(fileName.EndsWith))
		{
			return await GetCompressedImageAsync(fileBytes);
		}
		// TODO: REVIEW VIDEO COMPRESSION
		//else if (_videoExtensions.Any(fileName.EndsWith))
		//{
		//	return await GetCompressedVideoAsync(fileBytes, fileName);
		//}

		return new MemoryStream(fileBytes);
	}

	private static async Task<MemoryStream> GetCompressedImageAsync(byte[] fileBytes)
	{
		MemoryStream inputStream = new MemoryStream(fileBytes);
		MemoryStream outputStream = new MemoryStream();

		using (Image image = Image.Load(inputStream))
		{
			var encoder = new JpegEncoder { Quality = 40 };

			image.Mutate(x => x.Resize(new ResizeOptions
			{
				Mode = ResizeMode.Max,
				Size = new Size(image.Width, image.Height),
			}));

			await image.SaveAsync(outputStream, encoder);
		}

		outputStream.Seek(0, SeekOrigin.Begin);

		return outputStream;
	}

	private async Task<MemoryStream> GetCompressedVideoAsync(byte[] fileBytes, string fileName)
	{
		string tempFilePath = Path.Combine(Path.GetTempPath(), fileName);
		await File.WriteAllBytesAsync(tempFilePath, fileBytes);

		string compressedFilePath = Path.Combine(Path.GetTempPath(), fileName);

		Process ffmpeg = new Process();
		ffmpeg.StartInfo.FileName = "ffmpeg";
		ffmpeg.StartInfo.Arguments = $"-i {tempFilePath} -b:v 1000k -vf scale=1280:-1 {compressedFilePath}";
		ffmpeg.StartInfo.RedirectStandardOutput = true;
		ffmpeg.StartInfo.RedirectStandardError = true;
		ffmpeg.StartInfo.UseShellExecute = false;
		ffmpeg.StartInfo.CreateNoWindow = true;
		ffmpeg.Start();

		await ffmpeg.WaitForExitAsync();

		var compressedFileBytes = await File.ReadAllBytesAsync(compressedFilePath);
		MemoryStream memoryStream = new(compressedFileBytes);

		File.Delete(tempFilePath);
		File.Delete(compressedFilePath);

		return memoryStream;
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
