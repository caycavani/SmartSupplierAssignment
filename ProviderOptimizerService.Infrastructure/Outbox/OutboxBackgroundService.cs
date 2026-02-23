
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;   // IServiceProvider.CreateScope()
using Microsoft.Extensions.Hosting;              // BackgroundService  <-- IMPORTANTE
using Microsoft.Extensions.Logging;              // ILogger<T>         <-- IMPORTANTE
using ProviderOptimizerService.Infrastructure.Outbox;
using ProviderOptimizerService.Infrastructure.Persistence;


namespace ProviderOptimizerService.Infrastructure.Outbox
{
	public sealed class OutboxBackgroundService : BackgroundService
	{
		private readonly IServiceProvider _sp;
		private readonly ILogger<OutboxBackgroundService> _logger;
		private readonly TimeSpan _interval = TimeSpan.FromSeconds(5);

		public OutboxBackgroundService(IServiceProvider sp, ILogger<OutboxBackgroundService> logger)
		{
			_sp = sp; _logger = logger;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			_logger.LogInformation("Outbox dispatcher iniciado.");
			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					using var scope = _sp.CreateScope();
					var db = scope.ServiceProvider.GetRequiredService<OptimizerDbContext>();
					var dispatcher = scope.ServiceProvider.GetRequiredService<IEventDispatcher>();

					var batch = await db.OutboxMessages
						.Where(m => m.ProcessedOnUtc == null && m.Attempts < 10)
						.OrderBy(m => m.OccurredOnUtc)
						.Take(50)
						.ToListAsync(stoppingToken);

					foreach (var msg in batch)
					{
						try
						{
							await dispatcher.DispatchAsync(msg.Type, msg.Payload, msg.CorrelationId, msg.TraceId, stoppingToken);
							msg.ProcessedOnUtc = DateTime.UtcNow;
						}
						catch (Exception ex)
						{
							msg.Attempts += 1;
							msg.Error = ex.Message;
							_logger.LogError(ex, "Error publicando mensaje Outbox {Id}", msg.Id);
						}
					}

					await db.SaveChangesAsync(stoppingToken);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Fallo ciclo Outbox.");
				}

				await Task.Delay(_interval, stoppingToken);
			}
		}
	}
}