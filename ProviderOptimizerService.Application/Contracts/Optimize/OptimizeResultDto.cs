using System.Collections.Generic;

namespace ProviderOptimizerService.Application.Contracts.Optimize
{
	public sealed class OptimizeResultDto
	{
		public string AssistanceId { get; init; } = default!;
		public string ProviderId { get; init; } = default!;
		public int? EtaMinutes { get; init; }
		public double Score { get; init; }
		public IReadOnlyCollection<ScoreDetailDto> Explanation { get; init; } = new List<ScoreDetailDto>();

		public sealed class ScoreDetailDto
		{
			public string Name { get; init; } = default!;
			public double Weight { get; init; }
			public double Raw { get; init; }
			public double Normalized { get; init; }
			public double Contribution { get; init; }
		}
	}
}
