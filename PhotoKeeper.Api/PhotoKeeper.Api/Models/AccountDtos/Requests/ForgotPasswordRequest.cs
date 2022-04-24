namespace PhotoKeeper.Api.Models.AccountDtos.Requests;

using System.ComponentModel.DataAnnotations;

public class ForgotPasswordRequest
{
	[Required]
	[EmailAddress]
	public string Email { get; set; }
}
