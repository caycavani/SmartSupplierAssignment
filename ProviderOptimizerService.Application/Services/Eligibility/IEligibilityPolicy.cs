using ProviderOptimizerService.Domain.Model;

namespace ProviderOptimizerService.Application.Services.Eligibility
{
	public interface IEligibilityPolicy
	{
		bool IsEligible(Provider provider, Domain.Model.AssistanceRequest request);
	}
}