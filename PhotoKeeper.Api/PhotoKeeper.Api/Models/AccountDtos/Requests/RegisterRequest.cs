namespace PhotoKeeper.Api.Models.AccountDtos.Requests;

using System.ComponentModel.DataAnnotations;

public class RegisterRequest
{
	[Required]
	public string Name { get; set; }

	[Required]
	public string Surname { get; set; }

	[Required]
	[EmailAddress]
	public string Email { get; set; }

	[Required]
	[MinLength(5)]
	public string Password { get; set; }

	[Required]
	[Compare("Password")]
	public string ConfirmedPassword { get; set; }

	[Range(typeof(bool), "true", "true")]
	public bool AcceptTerms { get; set; }
}
