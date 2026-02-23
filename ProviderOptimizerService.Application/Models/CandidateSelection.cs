using System.Collections.Generic;

namespace ProviderOptimizerService.Application.Models
{
	public sealed record CandidateSelection(
		string ProviderId,
		double Score,
		IReadOnlyCollection<ScoreDetail> Explanation,
		int? EtaMinutes);
}