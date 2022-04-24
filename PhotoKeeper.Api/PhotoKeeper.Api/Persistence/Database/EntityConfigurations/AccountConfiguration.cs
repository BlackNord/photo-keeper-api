namespace PhotoKeeper.Api.Persistence.Database.EntityConfigurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhotoKeeper.Api.Entities;

public class AppointmentConfiguration : IEntityTypeConfiguration<Account>
{
	public void Configure(EntityTypeBuilder<Account> builder)
	{
		builder
			.Property(x => x.Name)
			.HasMaxLength(25);

		builder
			.Property(x => x.Surname)
			.HasMaxLength(25);

		builder
			.Property(x => x.Email)
			.HasMaxLength(50);
	}
}
