using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using ProviderOptimizerService.Application.Services;

// Alias explícitos a los contratos en Application
using AppOptimizeCommand = ProviderOptimizerService.Application.Contracts.Optimize.OptimizeCommand;
using AppOptimizeResultDto = ProviderOptimizerService.Application.Contracts.Optimize.OptimizeResultDto;

namespace ProviderOptimizerService.API.Controllers
{
	/// <summary>
	/// Endpoints de optimización (selección óptima de proveedor).
	/// </summary>
	[ApiController]
	[Route("[controller]")]
	[Produces(MediaTypeNames.Application.Json)]
	public sealed class OptimizeController : ControllerBase
	{
		private readonly OptimizeHandler _handler;

		public OptimizeController(OptimizeHandler handler) => _handler = handler;

		/// <summary>Ejecuta la optimización para una asistencia y devuelve el proveedor seleccionado.</summary>
		/// <remarks>
		/// - Usa la cabecera <b>Idempotency-Key</b> para respuestas idempotentes.<br/>
		/// - Propaga <b>X-Correlation-Id</b> y <b>X-Trace-Id</b> desde los headers si aplica.
		/// </remarks>
		[HttpPost]
		[ProducesResponseType(typeof(AppOptimizeResultDto), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Optimize([FromBody] AppOptimizeCommand command, CancellationToken ct)
		{
			// Idempotencia (header opcional)
			if (Request.Headers.TryGetValue("Idempotency-Key", out var idem))
				command = command with { IdempotencyKey = idem.ToString() };

			// CorrelationId (si no vino en el body)
			if (Request.Headers.TryGetValue("X-Correlation-Id", out var corr)
				&& string.IsNullOrWhiteSpace(command.CorrelationId))
				command = command with { CorrelationId = corr.ToString() };

			// TraceId (si no vino en el body)
			if (Request.Headers.TryGetValue("X-Trace-Id", out var trace)
				&& string.IsNullOrWhiteSpace(command.TraceId))
				command = command with { TraceId = trace.ToString() };

			var result = await _handler.HandleAsync(command, ct);
			return Ok(result);
		}
	}
}