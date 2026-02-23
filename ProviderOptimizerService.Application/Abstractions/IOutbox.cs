using System.Threading;
using System.Threading.Tasks;

namespace ProviderOptimizerService.Application.Abstractions
{
	/// <summary>
	/// Outbox para publicar eventos de integración de forma confiable.
	/// Implementación en Infrastructure (tabla outbox + dispatcher).
	/// </summary>
	public interface IOutbox
	{
		Task EnqueueAsync(object evt, string eventType, string? correlationId, string? traceId, CancellationToken ct);
	}
}