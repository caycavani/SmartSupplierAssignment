using System.Collections.Generic;
using ProviderOptimizerService.Application.Models;
using ProviderOptimizerService.Domain.Model;

namespace ProviderOptimizerService.Application.Services.Scoring
{
	public sealed class RatingStrategy : IScoringStrategy
	{
		public RatingStrategy(double weight) { Weight = weight; }
		public string Name => "rating";
		public double Weight { get; }
		public double Score(Provider provider, AssistanceRequest context, IList<ScoreDetail> _)
			=> System.Math.Clamp(provider.Rating / 5.0, 0, 1);
	}
}