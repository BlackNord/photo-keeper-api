namespace PhotoKeeper.Api.Entities;

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

[Owned]
public class RefreshToken
{
	[Key]
	public int Id { get; set; }

	public string Token { get; set; }
	public Account Account { get; set; }

	public DateTime CreationTime { get; set; }
	public string CreatedByIp { get; set; }

	public DateTime? CancellationTime { get; set; }
	public string? CancelledByIp { get; set; }
	public string? CancelReason { get; set; }

	public string? ReplacedByToken { get; set; }

	public DateTime ExpirationTime { get; set; }

	public bool IsExpired => DateTime.UtcNow >= ExpirationTime;
	public bool IsCancelled => CancellationTime.HasValue;
	public bool IsActive => !CancellationTime.HasValue && !IsExpired;
}
