using System;
using System.Globalization;

namespace ProviderOptimizerService.Domain.Model.ValueObjects
{
	/// <summary>
	/// Value Object de punto geográfico con distancia Haversine (km).
	/// </summary>
	public readonly record struct GeoPoint(double Lat, double Lng)
	{
		public double DistanceKmTo(GeoPoint other)
		{
			const double R = 6371d; // Radio de la Tierra (km)
			var dLat = DegToRad(other.Lat - Lat);
			var dLon = DegToRad(other.Lng - Lng);

			var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
					Math.Cos(DegToRad(Lat)) * Math.Cos(DegToRad(other.Lat)) *
					Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

			var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
			return R * c;
		}

		private static double DegToRad(double deg) => deg * (Math.PI / 180d);

		public override string ToString() =>
			$"{Lat.ToString(CultureInfo.InvariantCulture)},{Lng.ToString(CultureInfo.InvariantCulture)}";
	}
}
