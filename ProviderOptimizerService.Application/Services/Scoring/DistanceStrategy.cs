using System.Collections.Generic;
using ProviderOptimizerService.Application.Models;
using ProviderOptimizerService.Domain.Model;

namespace ProviderOptimizerService.Application.Services.Scoring
{
	public sealed class DistanceStrategy : IScoringStrategy
	{
		private readonly double _maxDistanceKm;
		public DistanceStrategy(double weight, double maxDistanceKm = 50) { Weight = weight; _maxDistanceKm = maxDistanceKm; }
		public string Name => "distance";
		public double Weight { get; }
		public double Score(Provider provider, AssistanceRequest context, IList<ScoreDetail> _)
		{
			var km = provider.CurrentLocation.DistanceKmTo(context.Location);
			var normalized = System.Math.Clamp(1 - (km / _maxDistanceKm), 0, 1);
			return normalized;
		}
	}
}
