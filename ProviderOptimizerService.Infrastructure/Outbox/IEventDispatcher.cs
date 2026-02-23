namespace ProviderOptimizerService.Infrastructure.Outbox
{
	/// <summary>
	/// Contrato de publicación real. Sustituir por SNS/SQS, Kafka, etc.
	/// </summary>
	public interface IEventDispatcher
	{
		Task DispatchAsync(string type, string payload, string? correlationId, string? traceId, CancellationToken ct);
	}
}
