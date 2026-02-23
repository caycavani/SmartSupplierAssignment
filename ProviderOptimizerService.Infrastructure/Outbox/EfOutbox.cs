using System.Text.Json;
using ProviderOptimizerService.Application.Abstractions;
using ProviderOptimizerService.Infrastructure.Persistence;

namespace ProviderOptimizerService.Infrastructure.Outbox
{
	public sealed class EfOutbox : IOutbox
	{
		private readonly OptimizerDbContext _db;

		public EfOutbox(OptimizerDbContext db) => _db = db;

		public async Task EnqueueAsync(object evt, string eventType, string? correlationId, string? traceId, CancellationToken ct)
		{
			var msg = new OutboxMessage
			{
				Id = Guid.NewGuid(),
				Type = eventType,
				Payload = JsonSerializer.Serialize(evt),
				OccurredOnUtc = DateTime.UtcNow,
				CorrelationId = correlationId,
				TraceId = traceId,
				Attempts = 0
			};
			_db.OutboxMessages.Add(msg);
			await _db.SaveChangesAsync(ct);
		}
	}
}