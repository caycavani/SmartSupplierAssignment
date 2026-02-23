using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProviderOptimizerService.Infrastructure.Persistence.Configurations
{
	public sealed class IdempotencyRecordConfiguration : IEntityTypeConfiguration<IdempotencyRecord>
	{
		public void Configure(EntityTypeBuilder<IdempotencyRecord> b)
		{
			b.ToTable("idempotency_records");
			b.HasKey(x => x.Key);
			b.Property(x => x.Key).HasColumnName("key").HasMaxLength(120);
			b.Property(x => x.Response).HasColumnName("response").HasColumnType("text");
			b.Property(x => x.CreatedAtUtc).HasColumnName("created_at_utc");
			b.Property(x => x.ExpiresAtUtc).HasColumnName("expires_at_utc");
			b.HasIndex(x => x.ExpiresAtUtc);
		}
	}
}