using ExpoShared.Domain.EmailTemplates;

namespace ExpoApp.Domain.EmailTemplates;

public class ForgotPasswordTemplate : IForgotPasswordEmailTemplate
{
	public string Subject
	{
		get => "ExpoApp - Forgot password";
	}

	public string Body
	{
		get => @"<p>Hello,</p>

					<p>We received a request to reset your password for your ExpoApp account. To proceed, please click the link below:</p>

					<br>
					<br>

					<p style=""margin-top: 16px;"">
					  <a href=""{0}/{4}/{1}/{2}/{3}"" 
						 style=""background-color: #4CAF50; color: white; padding: 10px 20px; text-decoration: none; border-radius: 4px;"">
						Reset Password
					  </a>
					</p>

					<br>
					<br>

					<p>This link is valid for a limited time. If you did not request a password reset, please disregard this email, and your password will remain unchanged.</p>

					<p>Thank you for using ExpoApp!</p>

					<p>Best regards,<br>
					The Support Team</p>";
	}
}