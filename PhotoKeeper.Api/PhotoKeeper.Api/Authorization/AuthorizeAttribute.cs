namespace PhotoKeeper.Api.Authorization;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PhotoKeeper.Api.Entities;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
	private readonly IList<Role> _roles;

	public AuthorizeAttribute(params Role[] roles)
	{
		_roles = roles ?? new Role[] { };
	}

	public void OnAuthorization(AuthorizationFilterContext context)
	{
		// skipping authorization with [AllowAnonymous] attribute action
		var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();

		if (allowAnonymous)
			return;

		// authorization process
		var account = (Account)context.HttpContext.Items["Account"];

		if (account == null || (_roles.Any() && !_roles.Contains(account.Role)))
		{
			// not logged in or role is unauthorized
			context.Result = new JsonResult(new { message = "Unauthorized status." }) { StatusCode = StatusCodes.Status401Unauthorized };
		}
	}
}
