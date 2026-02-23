using System;

namespace ProviderOptimizerService.Application.IntegrationEvents
{
	/// <summary>
	/// Evento de integración que publicará el Outbox hacia SNS/SQS.
	/// </summary>
	public sealed class ProviderAssignedIntegrationEvent
	{
		public ProviderAssignedIntegrationEvent(
			string assistanceId,
			Guid providerId,
			int? etaMinutes,
			double score,
			string? correlationId,
			string? traceId)
		{
			AssistanceId = assistanceId;
			ProviderId = providerId;
			EtaMinutes = etaMinutes;
			Score = score;
			CorrelationId = correlationId;
			TraceId = traceId;
			OccurredOnUtc = DateTime.UtcNow;
		}

		public string AssistanceId { get; }
		public Guid ProviderId { get; }
		public int? EtaMinutes { get; }
		public double Score { get; }
		public string? CorrelationId { get; }
		public string? TraceId { get; }
		public DateTime OccurredOnUtc { get; }
		public string EventType => nameof(ProviderAssignedIntegrationEvent);
	}
}