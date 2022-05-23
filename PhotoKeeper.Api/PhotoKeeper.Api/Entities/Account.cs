namespace PhotoKeeper.Api.Entities;

using System.ComponentModel.DataAnnotations;

public class Account
{
	[Key]
	public int Id { get; set; }

	public string Name { get; set; }
	public string Surname { get; set; }
	public string Email { get; set; }
	public string PasswordHash { get; set; }
	public bool AcceptTerms { get; set; }

	public Role Role { get; set; }

	public DateTime CreationTime { get; set; }
	public DateTime? UpdationTime { get; set; }

	public DateTime? VerificationTime { get; set; }
	public string? VerificationToken { get; set; }
	public bool IsVerified => VerificationTime.HasValue || ResetPasswordTime.HasValue;

	public DateTime? ResetTokenExpiresTime { get; set; }
	public string? ResetToken { get; set; }

	public DateTime? ResetPasswordTime { get; set; }

	public List<RefreshToken> RefreshTokens { get; set; }

	public bool PossessToken(string token)
	{
		return this.RefreshTokens?.Find(x => x.Token == token) != null;
	}
}
