namespace ProviderOptimizerService.Infrastructure.Persistence
{
	public class OutboxMessage
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public string Type { get; set; } = default!;
		public string Payload { get; set; } = default!;
		public DateTime OccurredOnUtc { get; set; }
		public DateTime? ProcessedOnUtc { get; set; }
		public int Attempts { get; set; }
		public string? CorrelationId { get; set; }
		public string? TraceId { get; set; }
		public string? Error { get; set; }
	}
}
