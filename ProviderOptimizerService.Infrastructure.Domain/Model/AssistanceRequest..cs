using ProviderOptimizerService.Domain.Abstractions;
using ProviderOptimizerService.Domain.Model.Enums;
using ProviderOptimizerService.Domain.Model.ValueObjects;

namespace ProviderOptimizerService.Domain.Model
{
	/// <summary>
	/// Solicitud de asistencia (Aggregate Root de dominio).
	/// </summary>
	public sealed class AssistanceRequest : AggregateRoot
	{
		/// <summary>
		/// Identificador de negocio de la solicitud (radicado externo).
		/// </summary>
		public string AssistanceId { get; private set; }

		public ServiceType ServiceType { get; private set; }

		public GeoPoint Location { get; private set; }

		public Constraints Constraints { get; private set; }

		private AssistanceRequest() { } // Para EF/serialización si lo necesitas en Infrastructure

		public AssistanceRequest(
			string assistanceId,
			ServiceType serviceType,
			GeoPoint location,
			Constraints? constraints)
		{
			if (string.IsNullOrWhiteSpace(assistanceId))
				throw new DomainException("AssistanceId es requerido.");

			AssistanceId = assistanceId.Trim();
			ServiceType = serviceType;
			Location = location;
			Constraints = constraints ?? new Constraints();
		}
	}
}