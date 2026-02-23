using ProviderOptimizerService.Domain.Model.Enums;

namespace ProviderOptimizerService.Application.Contracts.Providers
{
	public sealed class GetAvailableProvidersQuery
	{
		public ServiceType ServiceType { get; init; }
		public double Lat { get; init; }
		public double Lng { get; init; }
		public double? MaxDistanceKm { get; init; }
	}
}