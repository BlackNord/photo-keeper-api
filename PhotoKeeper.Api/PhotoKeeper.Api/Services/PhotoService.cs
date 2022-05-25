namespace PhotoKeeper.Api.Services;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoKeeper.Api.Entities;
using PhotoKeeper.Api.Interfaces.Services;
using PhotoKeeper.Api.Models.AccountDtos.Responses;
using PhotoKeeper.Api.Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;

public class PhotoService : IPhotoService
{
	private readonly ApplicationDatabaseContext _context;

	private readonly IWebHostEnvironment _hostEnvironment;

	private readonly IMapper _mapper;

	public PhotoService(ApplicationDatabaseContext context, IWebHostEnvironment hostEnvironment, IMapper mapper)
	{
		_context = context;
		_hostEnvironment = hostEnvironment;
		_mapper = mapper;
	}

	public IEnumerable<PhotoResponse> GetAll(int accountId, string scheme, HostString host, PathString pathBase)
	{
		var photos = _context.Photos
			.Select(x => new Photo()
			{
				Id = x.Id,
				PhotoName = x.PhotoName,
				Description = x.Description,
				ImageName = x.ImageName,
				ImageSrc = String.Format("{0}://{1}/Images/{3}", scheme, host, pathBase, x.ImageName),
				UserId = x.UserId
			})
			.Where(x => x.UserId == accountId)
			.OrderBy(x => x.Id)
			.ProjectTo<PhotoResponse>(_mapper.ConfigurationProvider)
			.ToList();

		return photos;
	}

	public PhotoResponse GetById(int id)
	{
		var photo = _context.Photos.Find(id);

		if (photo == null)
			throw new KeyNotFoundException("Photo not found.");

		return _mapper.Map<PhotoResponse>(photo);
	}

	public PhotoResponse Update(int id, [FromForm] Photo photo)
	{
		if (photo.ImageFile != null)
		{
			DeleteImage(photo.ImageName);
			photo.ImageName = SaveImage(photo.ImageFile);
		}

		_context.Entry(photo).State = EntityState.Modified;

		try
		{
			_context.SaveChanges();
		}
		catch (DbUpdateConcurrencyException)
		{
			if (!PhotoExists(id))
				throw new KeyNotFoundException("Photo not found.");
		}

		return _mapper.Map<PhotoResponse>(photo);
	}

	public PhotoResponse Create([FromForm] Photo photo)
	{
		photo.ImageName = SaveImage(photo.ImageFile);

		_context.Photos.Add(photo);
		_context.SaveChanges();

		return _mapper.Map<PhotoResponse>(photo);
	}

	public void Delete(int id)
	{
		var photo = _context.Photos.Find(id);

		_context.Photos.Remove(photo);
		_context.SaveChanges();

		DeleteImage(photo.ImageName);
	}

	// helpering

	private bool PhotoExists(int id)
	{
		return _context.Photos.Any(e => e.Id == id);
	}

	public string SaveImage(IFormFile imageFile)
	{
		string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
		imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);

		var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", imageName);
		using (var fileStream = new FileStream(imagePath, FileMode.Create))
		{
			imageFile.CopyTo(fileStream);
		}

		return imageName;
	}

	public void DeleteImage(string imageName)
	{
		var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", imageName);
		if (System.IO.File.Exists(imagePath))
			System.IO.File.Delete(imagePath);
	}
}
