namespace PhotoKeeper.Api.Models.AccountDtos.Requests;

using System.ComponentModel.DataAnnotations;

public class ResetPasswordRequest
{
	[Required]
	public string Token { get; set; }

	[Required]
	[MinLength(5)]
	public string Password { get; set; }

	[Required]
	[Compare("Password")]
	public string ConfirmedPassword { get; set; }
}
