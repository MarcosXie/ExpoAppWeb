using Amazon.Runtime;
using Microsoft.Extensions.Configuration;

namespace UExpo.Infrastructure.Utils;

public class AwsUtils
{
    public static BasicAWSCredentials? GetAwsCredentials(IConfiguration config)
    {
        var accessKey = config["AWS:AccessKey"];
        var secretKey = config["AWS:SecretKey"];

        return accessKey is not null ?
            new BasicAWSCredentials(accessKey, secretKey) :
            null;
    }
}
