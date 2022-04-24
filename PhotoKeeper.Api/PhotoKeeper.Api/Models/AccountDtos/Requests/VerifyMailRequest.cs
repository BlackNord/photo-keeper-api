namespace PhotoKeeper.Api.Models.AccountDtos.Requests;

using System.ComponentModel.DataAnnotations;

public class VerifyMailRequest
{
	[Required]
	public string Token { get; set; }
}
