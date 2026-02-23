using Microsoft.EntityFrameworkCore;
using ProviderOptimizerService.Domain.Model;
using ProviderOptimizerService.Domain.Model.Enums;
using ProviderOptimizerService.Domain.Model.ValueObjects;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ProviderOptimizerService.Infrastructure.Persistence
{
	public class OptimizerDbContext : DbContext
	{
		public OptimizerDbContext(DbContextOptions<OptimizerDbContext> options) : base(options) { }

		public DbSet<Provider> Providers => Set<Provider>();
		public DbSet<AssistanceRequest> AssistanceRequests => Set<AssistanceRequest>();

		// Infra entities (idempotency & outbox)
		public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
		public DbSet<IdempotencyRecord> IdempotencyRecords => Set<IdempotencyRecord>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new Configurations.ProviderConfiguration());
			modelBuilder.ApplyConfiguration(new Configurations.ProviderCapabilityConfiguration());
			modelBuilder.ApplyConfiguration(new Configurations.AssistanceRequestConfiguration());
			modelBuilder.ApplyConfiguration(new Configurations.OutboxMessageConfiguration());
			modelBuilder.ApplyConfiguration(new Configurations.IdempotencyRecordConfiguration());
		}
	}
}