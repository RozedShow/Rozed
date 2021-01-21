using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Servicios
{
    public class RChanBackgroundService : IHostedService, IDisposable
    {
        private readonly IServiceProvider services;
        private readonly ILogger<RChanBackgroundService> logger;
        private Timer timer;

        public RChanBackgroundService(IServiceProvider services, ILogger<RChanBackgroundService> logger)
        {
            this.services = services;
            this.logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(async (state) => await LimpearHilosViejos(), null, 0, (int) TimeSpan.FromHours(1).TotalMilliseconds);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public async Task LimpearHilosViejos() 
        {
            logger.LogInformation("[RBS] Comenazondo limpieza de hilos viejos...");
            using var scope = services.CreateScope();
            var hiloService = scope.ServiceProvider.GetService<IHiloService>();
            await hiloService.LimpiarHilosViejos();
            logger.LogInformation("[RBS] Limpieza terminada");
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}