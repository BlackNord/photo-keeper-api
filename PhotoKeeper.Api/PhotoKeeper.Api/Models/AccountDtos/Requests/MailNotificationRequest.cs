namespace PhotoKeeper.Api.Models.AccountDtos.Requests;

using System.ComponentModel.DataAnnotations;

public class MailNotificationRequest
{
	[Required]
	[EmailAddress]
	public string Email { get; set; }
}
