namespace PhotoKeeper.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using PhotoKeeper.Api.Interfaces.Services;

[ApiController]
[Route("[controller]")]
public class ColorizationController : BaseController
{

	private readonly IColorizationService _colorizationService;

	public ColorizationController(IColorizationService colorizationService)
	{
		_colorizationService = colorizationService;
	}

	[HttpPost("{id:int}")]
	public IActionResult Colorize(int id)
	{
		_colorizationService.Colorize(id);
		return Ok(new { message = "Photo was colorizated successfully." });
	}
}
