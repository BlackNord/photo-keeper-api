namespace PhotoKeeper.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using PhotoKeeper.Api.Entities;
using PhotoKeeper.Api.Interfaces.Services;
using PhotoKeeper.Api.Models.AccountDtos.Responses;
using System.Collections.Generic;

[ApiController]
[Route("[controller]")]
public class PhotoController : BaseController
{
	private readonly IPhotoService _photoService;

	public PhotoController(IPhotoService photoService)
	{
		_photoService = photoService;
	}

	[HttpGet]
	public ActionResult<IEnumerable<PhotoResponse>> GetAll()
	{
		var photos = _photoService.GetAll(Account.Id, Request.Scheme, Request.Host, Request.PathBase);
		return Ok(photos);
	}

	[HttpGet("{id:int}")]
	public ActionResult<PhotoResponse> GetById(int id)
	{
		var photo = _photoService.GetById(id);
		return Ok(photo);
	}

	[HttpPut("{id:int}")]
	public ActionResult<PhotoResponse> Update(int id, [FromForm] Photo photo)
	{
		if (id != photo.Id)
			return BadRequest();

		var model = _photoService.Update(id, photo);

		return Ok(model);
	}

	[HttpPost]
	public ActionResult<PhotoResponse> Create([FromForm] Photo photo)
	{
		var model = _photoService.Create(photo);
		return Ok(model);
	}

	[HttpDelete("{id:int}")]
	public IActionResult Delete(int id)
	{
		_photoService.Delete(id);
		return Ok(new { message = "Photo was deleted successfully." });
	}
}
