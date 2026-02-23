namespace ProviderOptimizerService.Application.Models
{
	public readonly record struct IdempotencyKey(string Value)
	{
		public override string ToString() => Value;
	}
}