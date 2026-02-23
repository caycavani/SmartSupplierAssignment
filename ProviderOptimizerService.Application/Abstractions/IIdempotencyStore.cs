using System.Threading;
using System.Threading.Tasks;

namespace ProviderOptimizerService.Application.Abstractions
{
	/// <summary>
	/// Almacén de idempotencia para POST /optimize (clave -> respuesta serializada).
	/// Implementación típica: Redis o tabla en BD.
	/// </summary>
	public interface IIdempotencyStore
	{
		Task<string?> TryGetAsync(string idempotencyKey, CancellationToken ct);
		Task SaveAsync(string idempotencyKey, string serializedResponse, int ttlSeconds, CancellationToken ct);
	}
}