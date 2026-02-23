using System;
using System.Collections.Generic;
using System.Linq;
using ProviderOptimizerService.Application.Models;
using ProviderOptimizerService.Domain.Model;

namespace ProviderOptimizerService.Application.Services.Scoring
{
	public sealed class CompositeScoringStrategy : IScoringStrategy
	{
		private readonly IReadOnlyCollection<IScoringStrategy> _strategies;

		public CompositeScoringStrategy(IEnumerable<IScoringStrategy> strategies)
		{
			_strategies = strategies?.ToArray() ?? Array.Empty<IScoringStrategy>();
			var sum = _strategies.Sum(s => s.Weight);
			if (Math.Abs(sum - 1.0) > 1e-6)
				throw new ArgumentException("La suma de pesos de estrategia debe ser 1.0");
		}

		public string Name => "composite";
		public double Weight => 1.0;

		public double Score(Provider provider, AssistanceRequest context, IList<ScoreDetail> explanation)
		{
			double total = 0;
			foreach (var s in _strategies)
			{
				var temp = new List<ScoreDetail>();
				var normalized = s.Score(provider, context, temp);
				var contrib = normalized * s.Weight;
				explanation.Add(new ScoreDetail(s.Name, s.Weight, normalized, normalized, contrib));
				total += contrib;
			}
			return Math.Clamp(total, 0, 1);
		}
	}
}