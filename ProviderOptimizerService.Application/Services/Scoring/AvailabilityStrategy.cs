using System.Collections.Generic;
using ProviderOptimizerService.Application.Models;
using ProviderOptimizerService.Domain.Model;

namespace ProviderOptimizerService.Application.Services.Scoring
{
	public sealed class AvailabilityStrategy : IScoringStrategy
	{
		public AvailabilityStrategy(double weight) { Weight = weight; }
		public string Name => "availability";
		public double Weight { get; }
		public double Score(Provider provider, AssistanceRequest context, IList<ScoreDetail> _)
		{
			// El repositorio YA filtró por ServiceType => aquí sólo validamos disponibilidad y cobertura.
			return (provider.IsAvailable && provider.Covers(context.Location)) ? 1.0 : 0.0;
		}
	}
}