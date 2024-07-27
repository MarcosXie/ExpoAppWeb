namespace UExpo.Domain.Email;

public class EmailSendDto
{
    public List<string> ToAddresses { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string Body { get; set; } = null!;
}
