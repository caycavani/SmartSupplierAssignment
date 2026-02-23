using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using ProviderOptimizerService.Application.Services;

// Alias a contratos Application
using AppGetAvailableProvidersQuery = ProviderOptimizerService.Application.Contracts.Providers.GetAvailableProvidersQuery;
using AppProviderDto = ProviderOptimizerService.Application.Contracts.Providers.ProviderDto;

namespace ProviderOptimizerService.API.Controllers
{
	/// <summary>
	/// Endpoints de consulta de proveedores.
	/// </summary>
	[ApiController]
	[Route("providers")]
	[Produces(MediaTypeNames.Application.Json)]
	public sealed class ProvidersController : ControllerBase
	{
		private readonly GetAvailableProvidersHandler _handler;

		public ProvidersController(GetAvailableProvidersHandler handler) => _handler = handler;

		/// <summary>Devuelve proveedores disponibles cerca de una ubicación.</summary>
		[HttpGet("available")]
		[ProducesResponseType(typeof(AppProviderDto[]), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetAvailable([FromQuery] AppGetAvailableProvidersQuery query, CancellationToken ct)
		{
			var items = await _handler.HandleAsync(query, ct);
			return Ok(items);
		}
	}
}