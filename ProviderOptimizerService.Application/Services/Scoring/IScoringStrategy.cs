using System.Collections.Generic;
using ProviderOptimizerService.Application.Models;
using ProviderOptimizerService.Domain.Model;

namespace ProviderOptimizerService.Application.Services.Scoring
{
	public interface IScoringStrategy
	{
		string Name { get; }
		double Weight { get; }  // 0..1 (la suma en Composite debe ser 1.0)
		double Score(Provider provider, AssistanceRequest context, IList<ScoreDetail> explanation);
	}
}