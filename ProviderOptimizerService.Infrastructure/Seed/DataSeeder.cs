using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql; // para capturar PostgresException 42P01
using ProviderOptimizerService.Domain.Model;
using ProviderOptimizerService.Domain.Model.Enums;
using ProviderOptimizerService.Domain.Model.ValueObjects;
using ProviderOptimizerService.Infrastructure.Persistence;

namespace ProviderOptimizerService.Infrastructure.Seed
{
	public static class DataSeeder
	{
		public static async Task SeedAsync(IServiceProvider sp, CancellationToken ct = default)
		{
			await using var scope = sp.CreateAsyncScope();
			var db = scope.ServiceProvider.GetRequiredService<OptimizerDbContext>();
			var logger = scope.ServiceProvider
							  .GetRequiredService<ILoggerFactory>()
							  .CreateLogger("DataSeeder");

			// 1) Migraciones: intenta, pero nunca caigas si no hay migraciones o algo falla.
			try
			{
				await db.Database.MigrateAsync(ct);
			}
			catch (InvalidOperationException ex)
			{
				// Suele ocurrir cuando el assembly no contiene migraciones compiladas.
				logger.LogWarning(ex, "Saltando Migrate(): no se encontraron migraciones en el assembly.");
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Fallo aplicando migraciones en SeedAsync (continuo sin detener la API).");
			}

			// 2) ¿Ya hay datos? si sí, salir.
			try
			{
				var exists = await db.Providers.AsNoTracking().AnyAsync(ct);
				if (exists)
				{
					logger.LogInformation("Seed omitido: ya existen proveedores.");
					return;
				}
			}
			catch (PostgresException pex) when (pex.SqlState == "42P01") // relation does not exist
			{
				logger.LogWarning("Tabla 'providers' no existe todavía (42P01). Omite seeding sin tumbar la app.");
				return;
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Fallo consultando existencia de 'providers'. Omite seeding.");
				return;
			}

			// 3) Insertar datos de ejemplo
			try
			{
				var p1 = new Provider("Taller Norte", new[] { ServiceType.Towing, ServiceType.TireChange },
					new GeoPoint(4.710989, -74.072090), rating: 4.6, costPerKm: 2.3, coverageRadiusKm: 25, isAvailable: true);
				var p2 = new Provider("Grúa Express", new[] { ServiceType.Towing, ServiceType.BatteryBoost },
					new GeoPoint(4.651, -74.12), rating: 4.2, costPerKm: 2.0, coverageRadiusKm: 30, isAvailable: true);
				var p3 = new Provider("Cerrajería 24h", new[] { ServiceType.Locksmith },
					new GeoPoint(4.65, -74.05), rating: 4.9, costPerKm: 3.1, coverageRadiusKm: 20, isAvailable: true);

				await db.Providers.AddRangeAsync(p1, p2, p3);

				// capacidades (join) si no usas shadow entities
				await db.Set<Infrastructure.Persistence.Configurations.ProviderCapability>().AddRangeAsync(
					new Infrastructure.Persistence.Configurations.ProviderCapability { ProviderId = p1.Id, ServiceType = (int)ServiceType.Towing },
					new Infrastructure.Persistence.Configurations.ProviderCapability { ProviderId = p1.Id, ServiceType = (int)ServiceType.TireChange },
					new Infrastructure.Persistence.Configurations.ProviderCapability { ProviderId = p2.Id, ServiceType = (int)ServiceType.Towing },
					new Infrastructure.Persistence.Configurations.ProviderCapability { ProviderId = p2.Id, ServiceType = (int)ServiceType.BatteryBoost },
					new Infrastructure.Persistence.Configurations.ProviderCapability { ProviderId = p3.Id, ServiceType = (int)ServiceType.Locksmith });

				await db.SaveChangesAsync(ct);
				logger.LogInformation("Seed completado: proveedores insertados.");
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Fallo insertando seed. Continuo sin detener la API.");
			}
		}
	}
}