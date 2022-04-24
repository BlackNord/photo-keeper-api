namespace PhotoKeeper.Api.Services;

using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using PhotoKeeper.Api.Interfaces.Services;
using PhotoKeeper.Api.Persistence.Application;

public class EmailService : IEmailService
{
	private readonly AppSettings _appSettings;

	public EmailService(IOptions<AppSettings> appSettings)
	{
		_appSettings = appSettings.Value;
	}

	public void Send(string to, string subject, string html, string from = null)
	{
		// create message
		var email = new MimeMessage();
		email.From.Add(MailboxAddress.Parse(from ?? _appSettings.EmailAddress));
		email.To.Add(MailboxAddress.Parse(to));
		email.Subject = subject;
		email.Body = new TextPart(TextFormat.Html) { Text = html };

		// send email
		using var smtp = new SmtpClient();
		smtp.Connect(_appSettings.SmtpHost, _appSettings.SmtpPort, SecureSocketOptions.Auto);
		smtp.Send(email);
		smtp.Disconnect(true);
	}
}
