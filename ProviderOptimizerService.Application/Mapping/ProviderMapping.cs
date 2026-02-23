using System.Linq;
using ProviderOptimizerService.Application.Contracts.Providers;
using ProviderOptimizerService.Domain.Model;

namespace ProviderOptimizerService.Application
{
	internal static class ProviderMapping
	{
		public static ProviderDto ToDto(Provider p) => new ProviderDto
		{
			ProviderId = p.Id.ToString(),
			Name = p.Name,
			Lat = p.CurrentLocation.Lat,
			Lng = p.CurrentLocation.Lng,
			IsAvailable = p.IsAvailable,
			Rating = p.Rating,
			CostPerKm = p.CostPerKm,
			CoverageRadiusKm = p.CoverageRadiusKm,
			Capabilities = p.Capabilities.ToList()
		};
	}
}