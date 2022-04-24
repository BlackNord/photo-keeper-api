namespace PhotoKeeper.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using PhotoKeeper.Api.Entities;

[Controller]
public abstract class BaseController : ControllerBase
{
	// returns the current authenticated account (null if not logged in)
	public Account Account => (Account)HttpContext.Items["Account"];
}
