using Amazon.Runtime;
using Microsoft.Extensions.Configuration;

namespace UExpo.Infrastructure.Utils;

public class AwsUtils
{
    public static BasicAWSCredentials? GetAwsCredentials(IConfiguration config)
    {
        string? accessKey = config["AWS:AccessKey"];
        string? secretKey = config["AWS:SecretKey"];

        return accessKey is not null ?
            new BasicAWSCredentials(accessKey, secretKey) :
            null;
    }
}
