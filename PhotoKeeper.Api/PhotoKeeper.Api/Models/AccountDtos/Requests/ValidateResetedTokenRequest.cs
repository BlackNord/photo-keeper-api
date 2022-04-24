namespace PhotoKeeper.Api.Models.AccountDtos.Requests;

using System.ComponentModel.DataAnnotations;

public class ValidateResetedTokenRequest
{
	[Required]
	public string Token { get; set; }
}
