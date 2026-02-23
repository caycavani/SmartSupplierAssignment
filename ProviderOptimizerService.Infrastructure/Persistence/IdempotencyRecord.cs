namespace ProviderOptimizerService.Infrastructure.Persistence
{
	public class IdempotencyRecord
	{
		public string Key { get; set; } = default!;
		public string Response { get; set; } = default!;
		public DateTime CreatedAtUtc { get; set; }
		public DateTime ExpiresAtUtc { get; set; }
	}
}