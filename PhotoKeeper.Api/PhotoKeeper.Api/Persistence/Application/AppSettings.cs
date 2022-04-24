namespace PhotoKeeper.Api.Persistence.Application;

public class AppSettings
{
	public string SecretWord { get; set; }

	// refresh token Time-To-Live(days)
	// tokens are deleted automaticly from the database after this time
	public int RefreshTokenTTL { get; set; }

	public string EmailAddress { get; set; }
	public string SmtpHost { get; set; }
	public int SmtpPort { get; set; }
	public string? SmtpUser { get; set; }
	public string? SmtpPassword { get; set; }
}
