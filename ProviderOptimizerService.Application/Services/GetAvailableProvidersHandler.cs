using ProviderOptimizerService.Application.Contracts.Providers;
using ProviderOptimizerService.Application.Ports;
using ProviderOptimizerService.Domain.Model.ValueObjects;

namespace ProviderOptimizerService.Application.Services
{
	public sealed class GetAvailableProvidersHandler
	{
		private readonly IProviderRepository _providers;

		public GetAvailableProvidersHandler(IProviderRepository providers)
		{
			_providers = providers;
		}

		public async Task<ProviderDto[]> HandleAsync(GetAvailableProvidersQuery query, CancellationToken ct)
		{
			var near = new GeoPoint(query.Lat, query.Lng);
			var list = await _providers.GetAvailableAsync(query.ServiceType, near, query.MaxDistanceKm, ct);

			return list.Select(ProviderMapping.ToDto).ToArray();
		}
	}
}