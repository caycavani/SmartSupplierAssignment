namespace ProviderOptimizerService.Application.Models
{
	public sealed record ScoreDetail(string Name, double Weight, double Raw, double Normalized, double Contribution);
}