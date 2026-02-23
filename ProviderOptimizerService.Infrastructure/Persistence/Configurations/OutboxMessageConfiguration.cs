using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProviderOptimizerService.Infrastructure.Persistence.Configurations
{
	public sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
	{
		public void Configure(EntityTypeBuilder<OutboxMessage> b)
		{
			b.ToTable("outbox_messages");
			b.HasKey(x => x.Id);
			b.Property(x => x.Id).HasColumnName("id");
			b.Property(x => x.Type).HasColumnName("type").HasMaxLength(250).IsRequired();
			b.Property(x => x.Payload).HasColumnName("payload").HasColumnType("json");
			b.Property(x => x.OccurredOnUtc).HasColumnName("occurred_on_utc").IsRequired();
			b.Property(x => x.ProcessedOnUtc).HasColumnName("processed_on_utc");
			b.Property(x => x.Attempts).HasColumnName("attempts").HasDefaultValue(0);
			b.Property(x => x.CorrelationId).HasColumnName("correlation_id").HasMaxLength(100);
			b.Property(x => x.TraceId).HasColumnName("trace_id").HasMaxLength(100);
			b.Property(x => x.Error).HasColumnName("error");
			b.HasIndex(x => x.ProcessedOnUtc);
		}
	}
}