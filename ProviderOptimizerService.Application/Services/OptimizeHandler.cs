using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using ProviderOptimizerService.Application.Abstractions;
using ProviderOptimizerService.Application.Contracts.Optimize;
using ProviderOptimizerService.Application.IntegrationEvents;
using ProviderOptimizerService.Application.Models;
using ProviderOptimizerService.Application.Ports;
using ProviderOptimizerService.Application.Services.Eligibility;
using ProviderOptimizerService.Application.Services.Scoring;

using ProviderOptimizerService.Domain.Model;
using ProviderOptimizerService.Domain.Model.Enums;
using ProviderOptimizerService.Domain.Model.ValueObjects;

namespace ProviderOptimizerService.Application.Services
{
	/// <summary>
	/// Orquesta el proceso de optimización (POST /optimize) con idempotencia y outbox.
	/// </summary>
	public sealed class OptimizeHandler
	{
		private readonly IProviderRepository _providers;
		private readonly IAssistanceRequestRepository _requests;
		private readonly IEligibilityPolicy _eligibility;
		private readonly IScoringStrategy _scoring;
		private readonly IIdempotencyStore _idempotency;
		private readonly IOutbox _outbox;
		private readonly IUnitOfWork _uow;

		public OptimizeHandler(
			IProviderRepository providers,
			IAssistanceRequestRepository requests,
			IEligibilityPolicy eligibility,
			IScoringStrategy scoring,
			IIdempotencyStore idempotency,
			IOutbox outbox,
			IUnitOfWork uow)
		{
			_providers = providers;
			_requests = requests;
			_eligibility = eligibility;
			_scoring = scoring;
			_idempotency = idempotency;
			_outbox = outbox;
			_uow = uow;
		}

		public async Task<OptimizeResultDto> HandleAsync(OptimizeCommand cmd, CancellationToken ct)
		{
			// 1) Idempotencia (si hay clave)
			if (!string.IsNullOrWhiteSpace(cmd.IdempotencyKey))
			{
				var cached = await _idempotency.TryGetAsync(cmd.IdempotencyKey!, ct);
				if (cached is not null)
					return JsonSerializer.Deserialize<OptimizeResultDto>(cached)!;
			}

			// 2) Construir modelo de dominio de la solicitud (no tiene por qué persistirse aún)
			var request = await EnsureRequestAsync(cmd, ct);

			// 3) Obtener candidatos disponibles prefiltrados
			var near = new GeoPoint(cmd.Lat, cmd.Lng);
			var providers = await _providers.GetAvailableAsync(cmd.ServiceType, near, maxDistanceKm: null, ct);

			// 4) Elegibilidad + Scoring
			var shortlist = providers.Where(p => _eligibility.IsEligible(p, request)).ToList();
			if (shortlist.Count == 0)
				throw new System.InvalidOperationException("No hay proveedores elegibles para esta solicitud.");

			Provider? best = null;
			double bestScore = double.MinValue;
			var explanation = new List<ScoreDetail>();
			int? bestEta = null;

			foreach (var p in shortlist)
			{
				var details = new List<ScoreDetail>();
				var score = _scoring.Score(p, request, details);
				if (score > bestScore)
				{
					bestScore = score;
					best = p;
					explanation = details;

					// ETA coherente con EtaStrategy (35 km/h)
					var km = p.CurrentLocation.DistanceKmTo(request.Location);
					bestEta = (int)System.Math.Ceiling((km / 35.0) * 60.0);
				}
			}

			var selection = new CandidateSelection(best!.Id.ToString(), System.Math.Clamp(bestScore, 0, 1), explanation, bestEta);

			// 5) Armar DTO de salida
			var dto = new OptimizeResultDto
			{
				AssistanceId = request.AssistanceId,
				ProviderId = selection.ProviderId,
				EtaMinutes = selection.EtaMinutes,
				Score = selection.Score,
				Explanation = selection.Explanation
					.Select(e => new OptimizeResultDto.ScoreDetailDto
					{
						Name = e.Name,
						Weight = e.Weight,
						Raw = e.Raw,
						Normalized = e.Normalized,
						Contribution = e.Contribution
					}).ToList()
			};

			// 6) Outbox: evento de integración ProviderAssigned
			var evt = new ProviderAssignedIntegrationEvent(
				assistanceId: request.AssistanceId,
				providerId: System.Guid.Parse(selection.ProviderId),
				etaMinutes: selection.EtaMinutes,
				score: selection.Score,
				correlationId: cmd.CorrelationId,
				traceId: cmd.TraceId);

			await _outbox.EnqueueAsync(evt, evt.EventType, cmd.CorrelationId, cmd.TraceId, ct);

			// 7) Persistir (si decidiste guardar el request u otros cambios)
			await _uow.SaveChangesAsync(ct);

			// 8) Guardar respuesta en idempotencia (TTL 24h por ejemplo)
			if (!string.IsNullOrWhiteSpace(cmd.IdempotencyKey))
			{
				var payload = JsonSerializer.Serialize(dto);
				await _idempotency.SaveAsync(cmd.IdempotencyKey!, payload, ttlSeconds: 86400, ct);
			}

			return dto;
		}

		private async Task<AssistanceRequest> EnsureRequestAsync(OptimizeCommand cmd, CancellationToken ct)
		{
			var current = await _requests.GetByAssistanceIdAsync(cmd.AssistanceId, ct);
			if (current is not null)
				return current;

			var req = new AssistanceRequest(
				assistanceId: cmd.AssistanceId,
				serviceType: cmd.ServiceType,
				location: new GeoPoint(cmd.Lat, cmd.Lng),
				constraints: new Constraints(MaxEtaMinutes: cmd.MaxEtaMinutes, PreferredNetworks: null));

			await _requests.AddAsync(req, ct);
			return req;
		}
	}
}