using System.Collections.Generic;
using ProviderOptimizerService.Domain.Model.Enums;

namespace ProviderOptimizerService.Application.Contracts.Providers
{
	public sealed class ProviderDto
	{
		public string ProviderId { get; init; } = default!;
		public string Name { get; init; } = default!;
		public double Lat { get; init; }
		public double Lng { get; init; }
		public bool IsAvailable { get; init; }
		public double Rating { get; init; }
		public double CostPerKm { get; init; }
		public double CoverageRadiusKm { get; init; }
		public IReadOnlyCollection<ServiceType> Capabilities { get; init; } = new List<ServiceType>();
	}
}