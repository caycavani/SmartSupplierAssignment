using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProviderOptimizerService.Domain.Model;

namespace ProviderOptimizerService.Infrastructure.Persistence.Configurations
{
	public sealed class ProviderConfiguration : IEntityTypeConfiguration<Provider>
	{
		public void Configure(EntityTypeBuilder<Provider> b)
		{
			b.ToTable("providers");

			b.HasKey(p => p.Id);
			b.Property(p => p.Id).HasColumnName("id");

			b.Property(p => p.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
			b.Property(p => p.IsAvailable).HasColumnName("is_available").IsRequired();
			b.Property(p => p.Rating).HasColumnName("rating").IsRequired();
			b.Property(p => p.CostPerKm).HasColumnName("cost_per_km").IsRequired();
			b.Property(p => p.CoverageRadiusKm).HasColumnName("coverage_radius_km").IsRequired();

			// GeoPoint (record struct)
			b.ComplexProperty(p => p.CurrentLocation, nav =>
			{
				nav.Property(x => x.Lat).HasColumnName("lat").IsRequired();
				nav.Property(x => x.Lng).HasColumnName("lng").IsRequired();
			});

			b.HasMany<ProviderCapability>()
			 .WithOne()
			 .HasForeignKey(pc => pc.ProviderId)
			 .HasPrincipalKey(p => p.Id);
		}
	}

	public sealed class ProviderCapability
	{
		public Guid ProviderId { get; set; }
		public int ServiceType { get; set; }
	}

	public sealed class ProviderCapabilityConfiguration : IEntityTypeConfiguration<ProviderCapability>
	{
		public void Configure(EntityTypeBuilder<ProviderCapability> b)
		{
			b.ToTable("provider_capabilities");
			b.HasKey(x => new { x.ProviderId, x.ServiceType });
			b.Property(x => x.ProviderId).HasColumnName("provider_id");
			b.Property(x => x.ServiceType).HasColumnName("service_type");
		}
	}
}