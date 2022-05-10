namespace PhotoKeeper.Api.Models.AccountDtos.Requests;

using PhotoKeeper.Api.Entities;
using System.ComponentModel.DataAnnotations;

public class CreateRequest
{
	[Required]
	public string Name { get; set; }

	[Required]
	public string Surname { get; set; }

	[Required]
	[EmailAddress]
	public string Email { get; set; }

	[Required]
	[EnumDataType(typeof(Role))]
	public string Role { get; set; }

	[Required]
	[MinLength(5)]
	[MaxLength(15)]
	public string Password { get; set; }

	[Required]
	[Compare("Password")]
	public string ConfirmedPassword { get; set; }
}
