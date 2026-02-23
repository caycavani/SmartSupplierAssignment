using Microsoft.EntityFrameworkCore;
using ProviderOptimizerService.Application.Ports;
using ProviderOptimizerService.Domain.Model;
using ProviderOptimizerService.Domain.Model.Enums;
using ProviderOptimizerService.Domain.Model.ValueObjects;
using ProviderOptimizerService.Infrastructure.Persistence;
using ProviderOptimizerService.Infrastructure.Persistence.Configurations;

namespace ProviderOptimizerService.Infrastructure.Repositories
{
	public sealed class ProviderRepository : IProviderRepository
	{
		private readonly OptimizerDbContext _db;

		public ProviderRepository(OptimizerDbContext db) => _db = db;

		public async Task<IReadOnlyList<Provider>> GetAvailableAsync(
			ServiceType serviceType,
			GeoPoint near,
			double? maxDistanceKm,
			CancellationToken ct)
		{
			// Consulta base (EF)
			var query =
				from p in _db.Providers.AsNoTracking()
				join pc in _db.Set<ProviderCapability>() on p.Id equals pc.ProviderId
				where p.IsAvailable && pc.ServiceType == (int)serviceType
				select p;

			var list = await query.Distinct().ToListAsync(ct);

			// Filtro de distancia en memoria (independiente de proveedor)
			if (maxDistanceKm is not null)
			{
				list = list
					.Where(p => p.CurrentLocation.DistanceKmTo(near) <= maxDistanceKm.Value)
					.ToList();
			}

			return list;
		}

		public async Task<Provider?> GetByIdAsync(string providerId, CancellationToken ct)
		{
			if (!Guid.TryParse(providerId, out var id)) return null;
			return await _db.Providers.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, ct);
		}
	}
}