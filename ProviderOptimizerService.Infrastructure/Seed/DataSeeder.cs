using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProviderOptimizerService.Domain.Model;
using ProviderOptimizerService.Domain.Model.Enums;
using ProviderOptimizerService.Domain.Model.ValueObjects;
using ProviderOptimizerService.Infrastructure.Persistence;
using ProviderOptimizerService.Infrastructure.Persistence.Configurations;

namespace ProviderOptimizerService.Infrastructure.Seed
{
	public static class DataSeeder
	{
		public static async Task SeedAsync(IServiceProvider sp, CancellationToken ct = default)
		{
			using var scope = sp.CreateScope();
			var db = scope.ServiceProvider.GetRequiredService<OptimizerDbContext>();
			var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Seeder");

			await db.Database.MigrateAsync(ct);

			if (await db.Providers.AnyAsync(ct))
			{
				logger.LogInformation("Seed omitido: ya existen proveedores.");
				return;
			}

			var p1 = new Provider("Taller Norte", new[] { ServiceType.Towing, ServiceType.TireChange },
				new GeoPoint(4.710989, -74.072090), rating: 4.6, costPerKm: 2.3, coverageRadiusKm: 25, isAvailable: true);
			var p2 = new Provider("Grúa Express", new[] { ServiceType.Towing, ServiceType.BatteryBoost },
				new GeoPoint(4.651, -74.12), rating: 4.2, costPerKm: 2.0, coverageRadiusKm: 30, isAvailable: true);
			var p3 = new Provider("Cerrajería 24h", new[] { ServiceType.Locksmith },
				new GeoPoint(4.65, -74.05), rating: 4.9, costPerKm: 3.1, coverageRadiusKm: 20, isAvailable: true);

			await db.Providers.AddRangeAsync(p1, p2, p3);

			// capacidades (join)
			await db.Set<ProviderCapability>().AddRangeAsync(
				new ProviderCapability { ProviderId = p1.Id, ServiceType = (int)ServiceType.Towing },
				new ProviderCapability { ProviderId = p1.Id, ServiceType = (int)ServiceType.TireChange },
				new ProviderCapability { ProviderId = p2.Id, ServiceType = (int)ServiceType.Towing },
				new ProviderCapability { ProviderId = p2.Id, ServiceType = (int)ServiceType.BatteryBoost },
				new ProviderCapability { ProviderId = p3.Id, ServiceType = (int)ServiceType.Locksmith });

			await db.SaveChangesAsync();
			logger.LogInformation("Seed completado.");
		}
	}
}