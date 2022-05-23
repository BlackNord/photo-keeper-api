namespace PhotoKeeper.Api.Persistence.Database.EntityConfigurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhotoKeeper.Api.Entities;

public class PhotoConfiguration : IEntityTypeConfiguration<Photo>
{
	public void Configure(EntityTypeBuilder<Photo> builder)
	{
		builder
			.Property(x => x.PhotoName)
			.HasMaxLength(50);

		builder
			.Property(x => x.Description)
			.HasMaxLength(50);

		builder
			.Property(x => x.ImageName)
			.HasMaxLength(50);
	}
}
