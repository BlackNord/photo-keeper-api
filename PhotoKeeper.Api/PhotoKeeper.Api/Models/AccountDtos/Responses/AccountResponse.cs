namespace PhotoKeeper.Api.Models.AccountDtos.Responses;

public class AccountResponse
{
	public int Id { get; set; }

	public string Name { get; set; }
	public string Surname { get; set; }
	public string Email { get; set; }

	public string Role { get; set; }

	public DateTime CreationTime { get; set; }
	public DateTime? UpdationTime { get; set; }

	public bool IsVerified { get; set; }
}
