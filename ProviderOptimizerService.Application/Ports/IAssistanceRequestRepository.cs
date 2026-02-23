using System.Threading;
using System.Threading.Tasks;
using ProviderOptimizerService.Domain.Model;

namespace ProviderOptimizerService.Application.Ports
{
	public interface IAssistanceRequestRepository
	{
		/// <summary>
		/// GetByAssistanceIdAsync
		/// </summary>
		/// <param name="assistanceId"></param>
		/// <param name="ct"></param>
		/// <returns></returns>
		Task<AssistanceRequest?> GetByAssistanceIdAsync(string assistanceId, CancellationToken ct);
		/// <summary>
		/// AddAsync
		/// </summary>
		/// <param name="request"></param>
		/// <param name="ct"></param>
		/// <returns></returns>
		Task AddAsync(AssistanceRequest request, CancellationToken ct);
		
		Task UpdateAsync(AssistanceRequest request, CancellationToken ct);
	}
}
