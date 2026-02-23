using Microsoft.EntityFrameworkCore;
using ProviderOptimizerService.Application.Abstractions;
using ProviderOptimizerService.Infrastructure.Persistence;

namespace ProviderOptimizerService.Infrastructure.Idempotency
{
	public sealed class EfIdempotencyStore : IIdempotencyStore
	{
		private readonly OptimizerDbContext _db;

		public EfIdempotencyStore(OptimizerDbContext db) => _db = db;

		public async Task<string?> TryGetAsync(string idempotencyKey, CancellationToken ct)
		{
			var now = DateTime.UtcNow;
			var rec = await _db.IdempotencyRecords.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Key == idempotencyKey && x.ExpiresAtUtc > now, ct);
			return rec?.Response;
		}

		public async Task SaveAsync(string idempotencyKey, string serializedResponse, int ttlSeconds, CancellationToken ct)
		{
			var rec = new IdempotencyRecord
			{
				Key = idempotencyKey,
				Response = serializedResponse,
				CreatedAtUtc = DateTime.UtcNow,
				ExpiresAtUtc = DateTime.UtcNow.AddSeconds(ttlSeconds)
			};
			_db.IdempotencyRecords.Add(rec);
			await _db.SaveChangesAsync(ct);
		}
	}
}