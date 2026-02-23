using System.Collections.Generic;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ProviderOptimizerService.Infrastructure.Persistence.Converters
{
	/// <summary>
	/// Convierte colecciones de strings a JSON (string) y viceversa.
	/// Implementación sin parámetros opcionales para evitar CS0054 en Expression Trees.
	/// </summary>
	public static class PreferredNetworksConverter
	{
		public static readonly ValueConverter<IReadOnlyCollection<string>?, string?> ToJsonString =
			new ValueConverter<IReadOnlyCollection<string>?, string?>(
				v => Serialize(v),
				s => Deserialize(s));

		// Métodos simples, SIN parámetros opcionales

		private static string? Serialize(IReadOnlyCollection<string>? value)
		{
			if (value == null) return null;
			// usa la sobrecarga sin opciones
			return JsonSerializer.Serialize(value);
		}

		private static IReadOnlyCollection<string>? Deserialize(string? json)
		{
			if (json == null) return null;
			// usa la sobrecarga genérica sin opciones
			var list = JsonSerializer.Deserialize<List<string>>(json);
			return list; // puede ser null si el JSON venía vacío/mal formado
		}
	}
}