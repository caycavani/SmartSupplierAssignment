using Microsoft.Extensions.DependencyInjection;
using ProviderOptimizerService.Application.Abstractions;
using ProviderOptimizerService.Application.Services;
using ProviderOptimizerService.Application.Services.Eligibility;
using ProviderOptimizerService.Application.Services.Scoring;

namespace ProviderOptimizerService.API
{
	public static class ApplicationRegistration
	{
		public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
		{
			// Eligibility
			services.AddSingleton<IEligibilityPolicy, DefaultEligibilityPolicy>();

			// Scoring weights (suma 1.0)
			services.AddSingleton<IScoringStrategy>(sp =>
				new CompositeScoringStrategy(new IScoringStrategy[]
				{
					new AvailabilityStrategy(0.35),
					new DistanceStrategy(0.25, maxDistanceKm: 50),
					new EtaStrategy(0.20, maxEtaMin: 60),
					new RatingStrategy(0.20)
				}));

			// Handlers
			services.AddScoped<OptimizeHandler>();
			services.AddScoped<GetAvailableProvidersHandler>();

			return services;
		}
	}
}