using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Microsoft.Extensions.Configuration;
using UExpo.Domain.Email;
using UExpo.Infrastructure.Utils;

namespace UExpo.Infrastructure.Services;

public class EmailServiceAws : IEmailService
{
    private readonly IConfiguration _config;
    private readonly IAmazonSimpleEmailService _sesClient;

    public EmailServiceAws(IConfiguration config)
    {
        _config = config;

        _sesClient = new AmazonSimpleEmailServiceClient(
            AwsUtils.GetAwsCredentials(config),
            RegionEndpoint.GetBySystemName(_config["AWS:Region"])
        );
    }


    public async Task SendEmailAsync(EmailSendDto emailSendDto)
    {
        SendEmailRequest sendRequest = new SendEmailRequest
        {
            Source = _config["SES:SenderEmail"],
            Destination = new Destination
            {
                ToAddresses = emailSendDto.ToAddresses
            },
            Message = new Message
            {
                Subject = new Content(emailSendDto.Subject),
                Body = new Body
                {
                    Html = new Content(emailSendDto.Body)
                }
            }
        };

        try
        {
            SendEmailResponse response = await _sesClient.SendEmailAsync(sendRequest);
            Console.WriteLine("Email sent successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send email: {emailSendDto.Subject}");
            Console.WriteLine(ex.Message);
        }
    }
}
