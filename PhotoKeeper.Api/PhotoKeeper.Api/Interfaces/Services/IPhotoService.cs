namespace PhotoKeeper.Api.Interfaces.Services;

using Microsoft.AspNetCore.Mvc;
using PhotoKeeper.Api.Entities;
using PhotoKeeper.Api.Models.AccountDtos.Responses;

public interface IPhotoService
{
	IEnumerable<PhotoResponse> GetAll(int accountId, string scheme, HostString host, PathString pathBase);

	PhotoResponse GetById(int id);

	PhotoResponse Update(int id, [FromForm] Photo model);

	PhotoResponse Create([FromForm] Photo model);

	void Delete(int id);
}
