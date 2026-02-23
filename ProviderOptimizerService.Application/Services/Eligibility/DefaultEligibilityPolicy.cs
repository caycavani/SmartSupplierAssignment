using ProviderOptimizerService.Domain.Model;

namespace ProviderOptimizerService.Application.Services.Eligibility
{
	/// <summary>
	/// Regla simple: disponible, soporta el servicio y cubre la ubicación.
	/// </summary>
	public sealed class DefaultEligibilityPolicy : IEligibilityPolicy
	{
		public bool IsEligible(Provider provider, Domain.Model.AssistanceRequest request)
			=> provider.IsAvailable && provider.Supports(request.ServiceType) && provider.Covers(request.Location);
	}
}