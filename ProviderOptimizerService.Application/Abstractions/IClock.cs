using System;

namespace ProviderOptimizerService.Application.Abstractions
{
	public interface IClock
	{
		DateTime UtcNow { get; }
	}
}