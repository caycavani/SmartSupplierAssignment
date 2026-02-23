using System.Text;

namespace ProviderOptimizerService.Infrastructure.Outbox
{
	/// <summary>
	/// Demo: publica en consola. Reemplazar por un dispatcher SNS/SQS.
	/// </summary>
	public sealed class ConsoleEventDispatcher : IEventDispatcher
	{
		public Task DispatchAsync(string type, string payload, string? correlationId, string? traceId, CancellationToken ct)
		{
			var prefix = $"[{DateTime.UtcNow:O}] OUTBOX PUBLISH type={type} corr={correlationId} trace={traceId}";
			Console.WriteLine(prefix);
			Console.WriteLine(payload);
			return Task.CompletedTask;
		}
	}
}