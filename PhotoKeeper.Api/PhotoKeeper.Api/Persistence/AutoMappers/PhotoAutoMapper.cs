namespace PhotoKeeper.Api.Persistence.AutoMappers;

using AutoMapper;
using PhotoKeeper.Api.Entities;
using PhotoKeeper.Api.Models.AccountDtos.Responses;

public class PhotoAutoMapper : Profile
{
	// mappings between model and entity objects
	public PhotoAutoMapper()
	{
		CreateMap<Photo, PhotoResponse>();
		CreateMap<PhotoResponse, Photo>();
	}
}
