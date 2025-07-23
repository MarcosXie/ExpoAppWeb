using ExpoShared.Domain.EmailTemplates;

namespace ExpoApp.Domain.EmailTemplates;

public class ConfirmationEmailTemplate : IConfirmationEmailTemplate
{
	public string Subject
	{
		get => "Welcome {0} to ExpoApp! Please verify your email.";
	}

	public string Body
	{
		get => @"<p>Hello,</p>

				<p>Thank you for registering on our platform! To complete your registration and ensure the security of your account, please verify your email address by clicking the link below:</p>

				<br>
				<br>

				<p style=""margin-top: 16px;"">
				<a href=""{0}/025_verify_email/{1}/{2}/{3}""
			style=""background-color: #4CAF50; color: white; padding: 10px 20px; text-decoration: none; border-radius: 4px;"">
				Validate
				</a>
				</p>

				<br>
				<br>

				<p>This link is valid for a limited time, so we recommend completing the verification as soon as possible.</p>

				<p>If you did not request this verification, please disregard this message.</p>

				<p>Thank you for joining our platform!</p>

				<p>Best regards,<br>
				The Support Team</p>";
	}
}