namespace PhotoKeeper.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using PhotoKeeper.Api.Authorization;
using PhotoKeeper.Api.Entities;
using PhotoKeeper.Api.Interfaces.Services;
using PhotoKeeper.Api.Models.AccountDtos.Requests;
using PhotoKeeper.Api.Models.AccountDtos.Responses;

[Authorize]
[ApiController]
[Route("[controller]")]
public class AccountController : BaseController
{
	private readonly IAccountService _accountService;

	public AccountController(IAccountService accountService)
	{
		_accountService = accountService;
	}

	[AllowAnonymous]
	[HttpPost("authenticate")]
	public ActionResult<AuthenticateResponse> Authenticate(AuthenticateRequest model)
	{
		var response = _accountService.Authenticate(model, ipAddress());
		setTokenCookie(response.RefreshToken);

		return Ok(response);
	}

	[AllowAnonymous]
	[HttpPost("refresh-token")]
	public ActionResult<AuthenticateResponse> RefreshToken()
	{
		var refreshToken = Request.Cookies["refreshToken"];
		var response = _accountService.RefreshToken(refreshToken, ipAddress());
		setTokenCookie(response.RefreshToken);

		return Ok(response);
	}

	[HttpPost("revoke-token")]
	public IActionResult RevokeToken(CancelTokenRequest model)
	{
		// accept token from request body or cookie
		var token = model.Token ?? Request.Cookies["refreshToken"];

		if (string.IsNullOrEmpty(token))
			return BadRequest(new { message = "Token is required." });

		// users can revoke their tokens and admins can revoke any tokens
		if (!Account.PossessToken(token) && Account.Role != Role.Administrator)
			return Unauthorized(new { message = "Unauthorized status." });

		_accountService.RevokeToken(token, ipAddress());

		return Ok(new { message = "Token was revoked." });
	}

	[AllowAnonymous]
	[HttpPost("register")]
	public IActionResult Register(RegisterRequest model)
	{
		_accountService.Register(model, Request.Headers["origin"]);
		return Ok(new { message = "Registration is successful, please check your email for verification instructions." });
	}

	[AllowAnonymous]
	[HttpPost("verify-email")]
	public IActionResult VerifyEmail(VerifyMailRequest model)
	{
		_accountService.VerifyEmail(model.Token);
		return Ok(new { message = "Verification is successful, you can now log in." });
	}

	[AllowAnonymous]
	[HttpPost("forgot-password")]
	public IActionResult ForgotPassword(ForgotPasswordRequest model)
	{
		_accountService.ForgotPassword(model, Request.Headers["origin"]);
		return Ok(new { message = "Please check your email for password reset instructions." });
	}

	[AllowAnonymous]
	[HttpPost("validate-reset-token")]
	public IActionResult ValidateResetToken(ValidateResetedTokenRequest model)
	{
		_accountService.ValidateResetToken(model);
		return Ok(new { message = "Token is valid." });
	}

	[AllowAnonymous]
	[HttpPost("reset-password")]
	public IActionResult ResetPassword(ResetPasswordRequest model)
	{
		_accountService.ResetPassword(model);
		return Ok(new { message = "Password was reseted successfully, you can now log in." });
	}

	[Authorize(Role.Administrator)]
	[HttpGet]
	public ActionResult<IEnumerable<AccountResponse>> GetAll()
	{
		var accounts = _accountService.GetAll();
		return Ok(accounts);
	}

	[HttpGet("{id:int}")]
	public ActionResult<AccountResponse> GetById(int id)
	{
		// users can get their account and admins can get any account
		if (id != Account.Id && Account.Role != Role.Administrator)
			return Unauthorized(new { message = "Unauthorized status." });

		var account = _accountService.GetById(id);

		return Ok(account);
	}

	[Authorize(Role.Administrator)]
	[HttpPost]
	public ActionResult<AccountResponse> Create(CreateRequest model)
	{
		var account = _accountService.Create(model);
		return Ok(account);
	}

	[HttpPut("{id:int}")]
	public ActionResult<AccountResponse> Update(int id, UpdateRequest model)
	{
		// users can update their account and admins can update any account
		if (id != Account.Id && Account.Role != Role.Administrator)
			return Unauthorized(new { message = "Unauthorized status." });

		// only admins can update a role
		if (Account.Role != Role.Administrator)
			model.Role = null;

		var account = _accountService.Update(id, model);

		return Ok(account);
	}

	[HttpDelete("{id:int}")]
	public IActionResult Delete(int id)
	{
		// users can delete their account and admins can delete any account
		if (id != Account.Id && Account.Role != Role.Administrator)
			return Unauthorized(new { message = "Unauthorized status." });

		_accountService.Delete(id);

		return Ok(new { message = "Account was deleted successfully." });
	}

	// helpering
	private void setTokenCookie(string token)
	{
		var cookieOptions = new CookieOptions
		{
			HttpOnly = true,
			Expires = DateTime.UtcNow.AddDays(7)
		};
		Response.Cookies.Append("refreshToken", token, cookieOptions);
	}

	private string ipAddress()
	{
		if (Request.Headers.ContainsKey("X-Forwarded-For"))
			return Request.Headers["X-Forwarded-For"];
		else
			return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
	}
}
