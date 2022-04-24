namespace PhotoKeeper.Api.Authorization;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PhotoKeeper.Api.Entities;
using PhotoKeeper.Api.Interfaces.Authorization;
using PhotoKeeper.Api.Persistence.Application;
using PhotoKeeper.Api.Persistence.Database;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

public class JwtUtils : IJwtUtils
{
	private readonly ApplicationDatabaseContext _appDataContext;
	private readonly AppSettings _appSettings;

	public JwtUtils(
		ApplicationDatabaseContext context,
		IOptions<AppSettings> appSettings)
	{
		_appDataContext = context;
		_appSettings = appSettings.Value;
	}

	public string GenerateJwtToken(Account account)
	{
		// generate JWT token (valid for 15 minutes)
		var tokenHandler = new JwtSecurityTokenHandler();
		var key = Encoding.ASCII.GetBytes(_appSettings.SecretWord);
		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(new[] { new Claim("id", account.Id.ToString()) }),
			Expires = DateTime.UtcNow.AddMinutes(15),
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
		};
		var token = tokenHandler.CreateToken(tokenDescriptor);
		return tokenHandler.WriteToken(token);
	}

	public int? ValidateJwtToken(string token)
	{
		if (token == null)
			return null;

		var tokenHandler = new JwtSecurityTokenHandler();
		var key = Encoding.ASCII.GetBytes(_appSettings.SecretWord);

		try
		{
			tokenHandler.ValidateToken(token, new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(key),
				ValidateIssuer = false,
				ValidateAudience = false,
				// setting clockskew to 0 so tokens expire exactly at expiration time (without time errors)
				ClockSkew = TimeSpan.Zero
			}, out SecurityToken validatedToken);

			var jwtToken = (JwtSecurityToken)validatedToken;
			var accountId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

			// return account id from JWT token if validation successful
			return accountId;
		}
		catch
		{
			// return null if validation fails
			return null;
		}
	}

	public RefreshToken GenerateRefreshToken(string ipAddress)
	{
		var refreshToken = new RefreshToken
		{
			// token is a cryptographically strong random sequence of values
			Token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64)),
			// token is valid for 7 days
			ExpirationTime = DateTime.UtcNow.AddDays(7),
			CreationTime = DateTime.UtcNow,
			CreatedByIp = ipAddress
		};

		// if token is unique by checking against in DB
		var tokenIsUnique = !_appDataContext.Accounts.Any(a => a.RefreshTokens.Any(t => t.Token == refreshToken.Token));

		if (!tokenIsUnique)
			return GenerateRefreshToken(ipAddress);

		return refreshToken;
	}
}
