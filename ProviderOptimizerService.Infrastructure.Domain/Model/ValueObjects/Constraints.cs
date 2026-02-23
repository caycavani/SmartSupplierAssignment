using System.Collections.Generic;

namespace ProviderOptimizerService.Domain.Model.ValueObjects
{
	/// <summary>
	/// Restricciones declaradas por la solicitud (p. ej., ETA máxima, redes preferidas).
	/// </summary>
	public sealed record Constraints(
		int? MaxEtaMinutes = null,
		IReadOnlyCollection<string>? PreferredNetworks = null);
}