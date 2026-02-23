namespace ProviderOptimizerService.Application.Models
{
	public readonly record struct CorrelationId(string Value)
	{
		public override string ToString() => Value;
		public static CorrelationId New() => new(System.Guid.NewGuid().ToString("N"));
	}
}