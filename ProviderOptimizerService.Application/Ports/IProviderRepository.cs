using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ProviderOptimizerService.Domain.Model;
using ProviderOptimizerService.Domain.Model.Enums;
using ProviderOptimizerService.Domain.Model.ValueObjects;

namespace ProviderOptimizerService.Application.Ports
{
	public interface IProviderRepository
	{
		Task<IReadOnlyList<Provider>> GetAvailableAsync(
			ServiceType serviceType,
			GeoPoint near,
			double? maxDistanceKm,
			CancellationToken ct);

		// Para seed/consultas administrativas (opcional)
		Task<Provider?> GetByIdAsync(string providerId, CancellationToken ct);
	}
}