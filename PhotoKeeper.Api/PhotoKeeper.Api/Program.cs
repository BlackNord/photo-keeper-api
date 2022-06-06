using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using PhotoKeeper.Api.Authorization;
using PhotoKeeper.Api.Interfaces.Authorization;
using PhotoKeeper.Api.Interfaces.Services;
using PhotoKeeper.Api.Persistence.Application;
using PhotoKeeper.Api.Persistence.Database;
using PhotoKeeper.Api.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// add services to DI container
{
	var services = builder.Services;
	var env = builder.Environment;

	services.AddDbContext<ApplicationDatabaseContext>();
	services.AddCors();
	services.AddControllers().AddJsonOptions(x =>
	{
		// serialize enums as strings in api responses (e.g. Role)
		x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
	});
	services.AddEndpointsApiExplorer();
	services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
	services.AddSwaggerGen();

	services.AddHttpLogging(options =>
	{
		options.LoggingFields = HttpLoggingFields.Request;
	});

	// configure strongly typed settings object
	services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
	services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));
	services.Configure<ClientSettings>(builder.Configuration.GetSection("ClientSettings"));

	// configure DI for application services and tools
	services.AddScoped<IJwtUtils, JwtUtils>();
	services.AddScoped<IAccountService, AccountService>();
	services.AddScoped<IEmailService, EmailService>();
	services.AddScoped<IPhotoService, PhotoService>();
	services.AddScoped<IColorizationService, ColorizationService>();
}

var app = builder.Build();

// migrate any database changes on startup (includes initial db creation)
using (var scope = app.Services.CreateScope())
{
	var dataContext = scope.ServiceProvider.GetRequiredService<ApplicationDatabaseContext>();
	dataContext.Database.Migrate();
}

// configure HTTP request pipeline
{
	// generated swagger json and swagger UI middleware
	app.UseForwardedHeaders(new ForwardedHeadersOptions
	{
		ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
	});

	app.UseSwagger();
	app.UseSwaggerUI(x => x.SwaggerEndpoint("/swagger/v1/swagger.json", "Photo Keeper API"));

	// global cors policy
	app.UseCors(x => x
		.SetIsOriginAllowed(origin => true)
		.AllowAnyMethod()
		.AllowAnyHeader()
		.AllowCredentials()
		.WithOrigins("http://localhost:8000"));

	// global error handler
	app.UseMiddleware<ErrorHandler>();

	// custom jwt auth middleware
	app.UseMiddleware<JwtMiddleware>();

	var env = builder.Environment;
	app.UseStaticFiles(new StaticFileOptions
	{
		FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "wwwroot", "Images")),
		RequestPath = "/wwwroot/Images"
	});

	app.UseHttpsRedirection();
	app.UseHttpLogging();
	app.UseAuthorization();
	app.MapControllers();
}

app.Run();
