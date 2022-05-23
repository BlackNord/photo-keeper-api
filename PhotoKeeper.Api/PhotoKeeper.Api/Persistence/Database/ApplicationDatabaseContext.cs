namespace PhotoKeeper.Api.Persistence.Database;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PhotoKeeper.Api.Entities;
using System.Reflection;

public class ApplicationDatabaseContext : DbContext
{
	public DbSet<Account>? Accounts { get; set; }
	public DbSet<Photo>? Photos { get; set; }

	private readonly DatabaseSettings settings;

	public ApplicationDatabaseContext(IOptions<DatabaseSettings> settings)
	{
		this.settings = settings.Value;
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder
			.UseNpgsql(settings.ConnectionString)
			.UseSnakeCaseNamingConvention()
			.UseLazyLoadingProxies();
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	}
}
