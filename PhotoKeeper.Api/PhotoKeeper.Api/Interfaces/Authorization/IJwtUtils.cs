namespace PhotoKeeper.Api.Interfaces.Authorization;

using PhotoKeeper.Api.Entities;

public interface IJwtUtils
{
	public string GenerateJwtToken(Account account);

	public int? ValidateJwtToken(string token);

	public RefreshToken GenerateRefreshToken(string ipAddress);
}
