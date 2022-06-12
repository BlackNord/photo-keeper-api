namespace PhotoKeeper.Api.Services;

using PhotoKeeper.Api.Interfaces.Services;
using PhotoKeeper.Api.Persistence.Database;
using System.Collections.Generic;
using System.Diagnostics;

public class ColorizationService : IColorizationService
{
	private readonly ApplicationDatabaseContext _context;

	private readonly IWebHostEnvironment _hostEnvironment;

	public ColorizationService(ApplicationDatabaseContext context, IWebHostEnvironment hostEnvironment)
	{
		_context = context;
		_hostEnvironment = hostEnvironment;
	}

	public void Colorize(int id)
	{
		var photo = _context.Photos.Find(id);
		if (photo == null)
			throw new KeyNotFoundException("Photo not found.");

		var imageOldName = photo.ImageName;

		CopyToNNInputFromStorage(imageOldName);

		RunColorizationNeuralNet();
		Thread.Sleep(3500);

		var imageName = imageOldName.Replace(".", DateTime.Now.ToString("yymmssfff") + ".");

		CopyToStorageFromNNOutput(imageOldName, imageName);

		photo.ImageName = imageName;
		photo.PhotoName = photo.PhotoName + "(colorized)";
		photo.Id = 0;

		_context.Photos.Add(photo);
		_context.SaveChanges();

		DeleteFromNNFolders(imageOldName);
	}

	public void CopyToNNInputFromStorage(string imageName)
	{
		var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", "Images", imageName);

		if (File.Exists(imagePath))
			File.Copy(imagePath, Path.Combine(_hostEnvironment.ContentRootPath, "Colorization/Input", imageName));
	}

	public void CopyToStorageFromNNOutput(string imageOldName, string imageName)
	{
		var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Colorization/Output", imageOldName);

		if (File.Exists(imagePath))
			File.Copy(imagePath, Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", "Images", imageName));
	}

	public void DeleteFromNNFolders(string imageOldName)
	{
		var imageInputPath = Path.Combine(_hostEnvironment.ContentRootPath, "Colorization/Input", imageOldName);
		var imageOutPath = Path.Combine(_hostEnvironment.ContentRootPath, "Colorization/Output", imageOldName);

		if (File.Exists(imageInputPath))
			File.Delete(imageInputPath);

		if (File.Exists(imageOutPath))
			File.Delete(imageOutPath);
	}

	public void RunColorizationNeuralNet()
	{
		using (Process process = new Process())
		{
			process.StartInfo.FileName = Path.Combine(_hostEnvironment.ContentRootPath, "Colorization/Colorization.exe");
			process.Start();
		}
	}
}
