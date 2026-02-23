using ProviderOptimizerService.Application.Abstractions;
using ProviderOptimizerService.Infrastructure.Persistence;

namespace ProviderOptimizerService.Infrastructure.Common
{
	public sealed class EfUnitOfWork : IUnitOfWork
	{
		private readonly OptimizerDbContext _db;
		public EfUnitOfWork(OptimizerDbContext db) => _db = db;
		public Task<int> SaveChangesAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
	}
}