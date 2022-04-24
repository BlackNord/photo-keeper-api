namespace PhotoKeeper.Api.Models.AccountDtos.Requests;

using PhotoKeeper.Api.Entities;
using System.ComponentModel.DataAnnotations;

public class UpdateRequest
{
	private string _password;
	private string _confirmedPassword;
	private string _role;
	private string _email;

	public string Name { get; set; }
	public string Surname { get; set; }

	[EnumDataType(typeof(Role))]
	public string Role
	{
		get => _role;
		set => _role = replaceEmptyWithNull(value);
	}

	[EmailAddress]
	public string Email
	{
		get => _email;
		set => _email = replaceEmptyWithNull(value);
	}

	[MinLength(5)]
	public string Password
	{
		get => _password;
		set => _password = replaceEmptyWithNull(value);
	}

	[Compare("Password")]
	public string ConfirmedPassword
	{
		get => _confirmedPassword;
		set => _confirmedPassword = replaceEmptyWithNull(value);
	}

	private string replaceEmptyWithNull(string value)
	{
		// replace empty string with null to make field optional
		return string.IsNullOrEmpty(value) ? null : value;
	}
}
