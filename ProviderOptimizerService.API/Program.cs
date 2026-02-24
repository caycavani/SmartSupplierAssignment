using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using ProviderOptimizerService.API;
using ProviderOptimizerService.Infrastructure;
using ProviderOptimizerService.Infrastructure.Seed;

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

	// XML comments (requiere <GenerateDocumentationFile>true</GenerateDocumentationFile> en el .csproj)
	var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
	if (File.Exists(xmlPath))
		c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
});

var app = builder.Build();

// ===================
//  DB MIGRATION & SEED
// ===================
// Regla: ejecuta Seed en Development o si SEED_ON_START=true (útil para ambientes de prueba)
var runSeed =
	app.Environment.IsDevelopment() ||
	string.Equals(Environment.GetEnvironmentVariable("SEED_ON_START"), "true", StringComparison.OrdinalIgnoreCase);

if (runSeed)
{
	try
	{
		await DataSeeder.SeedAsync(app.Services);
	}
	catch (Exception ex)
	{
		// No tumbar el host si algo falla en migración/seed
		var logger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger("Startup");
		logger.LogError(ex, "Fallo durante DB Migration & Seed (la API continúa ejecutándose).");
	}
}

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

// Endpoints básicos
app.MapControllers();

// Health (para checks en Docker/Orquestador)
app.MapGet("/health", () => Results.Ok(new { status = "OK", env = app.Environment.EnvironmentName }));

app.Run();

// 👇 Necesaria para WebApplicationFactory<Program> en tests de integración
public partial class Program { }