namespace UExpo.Domain.Email;

public interface IEmailService
{
    Task SendEmailAsync(EmailSendDto emailSendDto);
}
