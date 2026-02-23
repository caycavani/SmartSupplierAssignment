using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProviderOptimizerService.Application.Abstractions;
using ProviderOptimizerService.Application.Ports;
using ProviderOptimizerService.Infrastructure.Common;
using ProviderOptimizerService.Infrastructure.Idempotency;
using ProviderOptimizerService.Infrastructure.Outbox;
using ProviderOptimizerService.Infrastructure.Persistence;
using ProviderOptimizerService.Infrastructure.Repositories;

namespace ProviderOptimizerService.Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
		{
			var provider = config["Database:Provider"]?.ToLowerInvariant() ?? "postgres";
			var connStr = config.GetConnectionString("Default")!;

			services.AddDbContext<OptimizerDbContext>(opt =>
			{
				
					opt.UseNpgsql(connStr, m => m.MigrationsHistoryTable("__EFMigrationsHistory"));
			});

			services.AddScoped<IProviderRepository, ProviderRepository>();
			services.AddScoped<IAssistanceRequestRepository, AssistanceRequestRepository>();
			services.AddScoped<IUnitOfWork, EfUnitOfWork>();

			services.AddScoped<IIdempotencyStore, EfIdempotencyStore>();
			services.AddScoped<IOutbox, EfOutbox>();
			services.AddSingleton<IEventDispatcher, ConsoleEventDispatcher>();
			services.AddHostedService<OutboxBackgroundService>();

			return services;
		}
	}
}