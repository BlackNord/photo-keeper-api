namespace PhotoKeeper.Api.Interfaces.Services;

using PhotoKeeper.Api.Models.AccountDtos.Requests;
using PhotoKeeper.Api.Models.AccountDtos.Responses;

public interface IAccountService
{
	AuthenticateResponse Authenticate(AuthenticateRequest model, string ipAddress);
	AuthenticateResponse RefreshToken(string token, string ipAddress);

	void Register(RegisterRequest model);
	void VerifyEmail(string token);
	void RepeatVerifying(MailNotificationRequest model);

	void ForgotPassword(MailNotificationRequest model);
	void ResetPassword(ResetPasswordRequest model);

	void ValidateResetToken(ValidateResetedTokenRequest model);
	void RevokeToken(string token, string ipAddress);

	IEnumerable<AccountResponse> GetAll();

	AccountResponse GetById(int id);
	AccountResponse Create(CreateRequest model);
	AccountResponse Update(int id, UpdateRequest model);

	void Delete(int id);
}
