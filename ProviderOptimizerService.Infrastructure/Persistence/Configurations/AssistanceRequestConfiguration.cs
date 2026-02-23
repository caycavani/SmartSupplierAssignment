using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProviderOptimizerService.Domain.Model;
using ProviderOptimizerService.Infrastructure.Persistence.Converters;

namespace ProviderOptimizerService.Infrastructure.Persistence.Configurations
{
	public sealed class AssistanceRequestConfiguration : IEntityTypeConfiguration<AssistanceRequest>
	{
		public void Configure(EntityTypeBuilder<AssistanceRequest> b)
		{
			b.ToTable("assistance_requests");

			b.HasKey(a => a.Id);
			b.Property(a => a.Id).HasColumnName("id");

			b.Property(a => a.AssistanceId)
				.HasColumnName("assistance_id")
				.HasMaxLength(80)
				.IsRequired();

			b.Property(a => a.ServiceType)
				.HasColumnName("service_type")
				.IsRequired();

			// GeoPoint (record struct) => ComplexProperty (EF Core 8)
			b.ComplexProperty(a => a.Location, nav =>
			{
				nav.Property(p => p.Lat).HasColumnName("lat").IsRequired();
				nav.Property(p => p.Lng).HasColumnName("lng").IsRequired();
			});

			// Constraints => ComplexProperty + converter para PreferredNetworks
			b.ComplexProperty(a => a.Constraints, nav =>
			{
				nav.Property(p => p.MaxEtaMinutes)
				   .HasColumnName("max_eta_minutes");


				// PostgreSQL por defecto
				nav.Property(p => p.PreferredNetworks)
				   .HasColumnName("preferred_networks")
				   .HasColumnType("text")
				   .HasConversion(PreferredNetworksConverter.ToJsonString);

			});

			b.HasIndex(a => a.AssistanceId).IsUnique();
		}
	}
}