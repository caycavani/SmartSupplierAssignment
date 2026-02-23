using System;

namespace ProviderOptimizerService.Domain.Abstractions
{
	/// <summary>
	/// Excepción de dominio para violaciones de invariantes o reglas.
	/// </summary>
	public class DomainException : Exception
	{
		public DomainException(string message) : base(message) { }
	}
}