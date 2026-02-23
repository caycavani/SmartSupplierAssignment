using System.Collections.Generic;
using ProviderOptimizerService.Application.Models;
using ProviderOptimizerService.Domain.Model;

namespace ProviderOptimizerService.Application.Services.Scoring
{
	public sealed class EtaStrategy : IScoringStrategy
	{
		private readonly int _maxEtaMin;
		private const double AvgSpeedKmH = 35.0;
		public EtaStrategy(double weight, int maxEtaMin = 60) { Weight = weight; _maxEtaMin = maxEtaMin; }
		public string Name => "eta";
		public double Weight { get; }

		public double Score(Provider provider, AssistanceRequest context, IList<ScoreDetail> _)
		{
			var km = provider.CurrentLocation.DistanceKmTo(context.Location);
			var eta = (int)System.Math.Ceiling((km / AvgSpeedKmH) * 60.0);
			var normalized = eta >= _maxEtaMin ? 0 : 1 - ((double)eta / _maxEtaMin);
			return System.Math.Clamp(normalized, 0, 1);
		}
	}
}