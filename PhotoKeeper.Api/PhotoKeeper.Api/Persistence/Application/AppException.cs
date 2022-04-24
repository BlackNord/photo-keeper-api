namespace PhotoKeeper.Api.Persistence.Application;

using System.Globalization;

// custom exception class for throwing specific exceptions from app
public class AppException : Exception
{
	public AppException() : base() { }

	public AppException(string message) : base(message) { }

	public AppException(string message, params object[] args)
		: base(String.Format(CultureInfo.CurrentCulture, message, args))
	{
	}
}
