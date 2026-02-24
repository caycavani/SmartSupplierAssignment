using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ProviderOptimizerService.Infrastructure.Persistence;

namespace ProviderOptimizerService.Infrastructure.Persistence
{
	/// <summary>
	/// Factory de diseño para dotnet-ef (migraciones / update).
	/// Permite construir OptimizerDbContext sin depender del host de la API.
	/// </summary>
	public sealed class OptimizerDbContextFactory : IDesignTimeDbContextFactory<OptimizerDbContext>
	{
		public OptimizerDbContext CreateDbContext(string[] args)
		{
			// Carga de configuración: appsettings.json (si existe) + variables de entorno.
			var basePath = Directory.GetCurrentDirectory();
			var config = new ConfigurationBuilder()
				.SetBasePath(basePath)
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
				.AddEnvironmentVariables()
				.Build();

			// Selección de provider (postgres por defecto).
			var provider = (config["Database:Provider"] ?? "postgres").ToLowerInvariant();

			// Connection string (usa la de config si existe; si no, una por defecto local).
			var conn = config.GetConnectionString("Default")
					  ?? "Host=127.0.0.1;Port=5432;Database=optimizer;Username=optimizer;Password=optimizer";

			var options = new DbContextOptionsBuilder<OptimizerDbContext>();

			
				options.UseNpgsql(conn, b => b.MigrationsHistoryTable("__EFMigrationsHistory"));
			

			return new OptimizerDbContext(options.Options);
		}
	}
}