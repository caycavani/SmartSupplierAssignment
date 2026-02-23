using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using ProviderOptimizerService.API;                 // AddApplicationLayer()
using ProviderOptimizerService.Infrastructure;      // AddInfrastructure()
using ProviderOptimizerService.Infrastructure.Seed; // DataSeeder

var builder = WebApplication.CreateBuilder(args);

// ===================
//   CONFIGURACIÓN
// ===================
builder.Configuration
	.AddJsonFile("appsettings.json", optional: true)
	.AddEnvironmentVariables();

// ===================
//       LAYERS
// ===================
builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructure(builder.Configuration);

// ===================
//     CONTROLLERS
// ===================
builder.Services.AddControllers()
	.AddJsonOptions(options =>
	{
		// Opcional:
		// options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
		// options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
	});

// ===================
//      SWAGGER
// ===================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo
	{
		Title = "Provider Optimizer Service API",
		Version = "v1",
		Description = "Microservicio crítico: ProviderOptimizerService (Clean/Hexagonal, Outbox, Idempotencia)."
	});

	// XML comments (requiere <GenerateDocumentationFile>true</GenerateDocumentationFile> en csproj)
	var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
	if (File.Exists(xmlPath))
		c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
});



// (Opcional) CORS / Auth / RateLimit
// builder.Services.AddCors(...);
// builder.Services.AddAuthentication(...);
// builder.Services.AddAuthorization(...);

var app = builder.Build();

// ===================
//  DB MIGRATION & SEED
// ===================
await DataSeeder.SeedAsync(app.Services);

// ===================
//       PIPELINE
// ===================
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(options =>
	{
		options.SwaggerEndpoint("/swagger/v1/swagger.json", "Provider Optimizer Service API v1");
	});
}

// app.UseHttpsRedirection(); // opcional
// app.UseCors();
// app.UseAuthentication();
// app.UseAuthorization();

app.MapControllers();

app.Run();