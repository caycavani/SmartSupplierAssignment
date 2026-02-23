using ProviderOptimizerService.Domain.Model.Enums;

namespace ProviderOptimizerService.Application.Contracts.Optimize
{
	// Debe ser record (no class) para permitir 'with { ... }'
	public sealed record OptimizeCommand(
		string AssistanceId,
		ServiceType ServiceType,
		double Lat,
		double Lng,
		int? MaxEtaMinutes,
		string? CorrelationId = null,
		string? TraceId = null,
		string? IdempotencyKey = null
	);
}