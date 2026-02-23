using System.Collections.Generic;
using System.Linq;
using ProviderOptimizerService.Domain.Abstractions;
using ProviderOptimizerService.Domain.Model.Enums;
using ProviderOptimizerService.Domain.Model.ValueObjects;

namespace ProviderOptimizerService.Domain.Model
{
	/// <summary>
	/// Proveedor de servicio (Aggregate/Entity de dominio).
	/// </summary>
	public sealed class Provider : AggregateRoot
	{
		public string Name { get; private set; } = default!;

		/// <summary>
		/// Capacidades del proveedor (tipos de servicio).
		/// </summary>
		public HashSet<ServiceType> Capabilities { get; private set; } = new();

		/// <summary>
		/// Ubicación geográfica actual del proveedor.
		/// </summary>
		public GeoPoint CurrentLocation { get; private set; }

		/// <summary>
		/// Disponibilidad operativa del proveedor.
		/// </summary>
		public bool IsAvailable { get; private set; } = true;

		/// <summary>
		/// Calificación histórica del proveedor (0..5).
		/// </summary>
		public double Rating { get; private set; }

		/// <summary>
		/// Costo estimado por kilómetro.
		/// </summary>
		public double CostPerKm { get; private set; }

		/// <summary>
		/// Radio de cobertura en km (modelo simplificado).
		/// </summary>
		public double CoverageRadiusKm { get; private set; }

		private Provider() { } // Para EF/serialización si lo necesitas

		public Provider(
			string name,
			IEnumerable<ServiceType> capabilities,
			GeoPoint currentLocation,
			double rating,
			double costPerKm,
			double coverageRadiusKm,
			bool isAvailable = true)
		{
			Id = System.Guid.NewGuid();

			if (string.IsNullOrWhiteSpace(name))
				throw new DomainException("El nombre del proveedor es requerido.");

			Name = name.Trim();
			Capabilities = new HashSet<ServiceType>(capabilities ?? Enumerable.Empty<ServiceType>());
			CurrentLocation = currentLocation;
			Rating = System.Math.Clamp(rating, 0, 5);
			CostPerKm = costPerKm;
			CoverageRadiusKm = coverageRadiusKm;
			IsAvailable = isAvailable;
		}

		/// <summary>
		/// ¿Soporta el tipo de servicio?
		/// </summary>
		public bool Supports(ServiceType type) => Capabilities.Contains(type);

		/// <summary>
		/// ¿La ubicación objetivo está dentro del radio de cobertura?
		/// </summary>
		public bool Covers(GeoPoint target) =>
			CurrentLocation.DistanceKmTo(target) <= CoverageRadiusKm;

		/// <summary>
		/// Cambia la disponibilidad (regla simple del dominio).
		/// </summary>
		public void SetAvailability(bool available) => IsAvailable = available;

		/// <summary>
		/// Actualiza la ubicación del proveedor (por telemetría).
		/// </summary>
		public void UpdateLocation(GeoPoint location) => CurrentLocation = location;
	}
}