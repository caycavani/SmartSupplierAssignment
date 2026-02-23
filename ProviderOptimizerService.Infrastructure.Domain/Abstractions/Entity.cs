using System;

namespace ProviderOptimizerService.Domain.Abstractions
{
	/// <summary>
	/// Entidad base de dominio (sin eventos para mantener el dominio mínimo y puro).
	/// </summary>
	public abstract class Entity
	{
		/// <summary>
		/// Identificador único de la entidad dentro del dominio.
		/// </summary>
		public Guid Id { get; protected set; } = Guid.NewGuid();

		/// <summary>
		/// Dos entidades son iguales si comparten el mismo Id y tipo concreto.
		/// </summary>
		public override bool Equals(object? obj)
		{
			if (obj is not Entity other) return false;
			if (ReferenceEquals(this, other)) return true;
			if (GetType() != other.GetType()) return false;
			if (Id == Guid.Empty || other.Id == Guid.Empty) return false;
			return Id == other.Id;
		}

		public override int GetHashCode() => HashCode.Combine(GetType(), Id);
	}
}