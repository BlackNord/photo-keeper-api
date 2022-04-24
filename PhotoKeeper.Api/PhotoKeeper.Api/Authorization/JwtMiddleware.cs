namespace PhotoKeeper.Api.Authorization;

using Microsoft.Extensions.Options;
using PhotoKeeper.Api.Interfaces.Authorization;
using PhotoKeeper.Api.Persistence.Application;
using PhotoKeeper.Api.Persistence.Database;

public class JwtMiddleware
{
	private readonly RequestDelegate _next;
	private readonly AppSettings _appSettings;

	public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
	{
		_next = next;
		_appSettings = appSettings.Value;
	}

	public async Task Invoke(HttpContext httpContext, ApplicationDatabaseContext appDataContext, IJwtUtils jwtUtils)
	{
		var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

		var accountId = jwtUtils.ValidateJwtToken(token);
		if (accountId != null)
		{
			// attach account to context on successful JWT validation
			httpContext.Items["Account"] = await appDataContext.Accounts.FindAsync(accountId.Value);
		}

		await _next(httpContext);
	}
}
