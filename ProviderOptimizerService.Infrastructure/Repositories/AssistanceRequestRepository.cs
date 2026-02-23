using Microsoft.EntityFrameworkCore;
using ProviderOptimizerService.Application.Ports;
using ProviderOptimizerService.Domain.Model;
using ProviderOptimizerService.Infrastructure.Persistence;

namespace ProviderOptimizerService.Infrastructure.Repositories
{
	public sealed class AssistanceRequestRepository : IAssistanceRequestRepository
	{
		private readonly OptimizerDbContext _db;

		public AssistanceRequestRepository(OptimizerDbContext db) => _db = db;

		public async Task<AssistanceRequest?> GetByAssistanceIdAsync(string assistanceId, CancellationToken ct)
		{
			return await _db.AssistanceRequests
				.AsNoTracking()
				.FirstOrDefaultAsync(a => a.AssistanceId == assistanceId, ct);
		}

		public async Task AddAsync(AssistanceRequest request, CancellationToken ct)
		{
			await _db.AssistanceRequests.AddAsync(request, ct);
		}

		public Task UpdateAsync(AssistanceRequest request, CancellationToken ct)
		{
			_db.AssistanceRequests.Update(request);
			return Task.CompletedTask;
		}
	}
}