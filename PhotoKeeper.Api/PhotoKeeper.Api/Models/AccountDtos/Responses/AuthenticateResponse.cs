namespace PhotoKeeper.Api.Models.AccountDtos.Responses;

using System.Text.Json.Serialization;

public class AuthenticateResponse
{
	public int Id { get; set; }

	public string Name { get; set; }
	public string Surname { get; set; }
	public string Email { get; set; }

	public string Role { get; set; }

	public DateTime CreationTime { get; set; }
	public DateTime? UpdationTime { get; set; }

	public bool IsVerified { get; set; }

	public string JwtToken { get; set; }

	// refresh token is only returned in http cookie
	[JsonIgnore]
	public string RefreshToken { get; set; }
}
