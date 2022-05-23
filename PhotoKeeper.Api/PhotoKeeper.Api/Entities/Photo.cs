namespace PhotoKeeper.Api.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Photo
{
	[Key]
	public int Id { get; set; }

	public string PhotoName { get; set; }

	public string? Description { get; set; }

	public string ImageName { get; set; }

	[NotMapped]
	public IFormFile? ImageFile { get; set; }

	[NotMapped]
	public string? ImageSrc { get; set; }

	public int UserId { get; set; }
}
