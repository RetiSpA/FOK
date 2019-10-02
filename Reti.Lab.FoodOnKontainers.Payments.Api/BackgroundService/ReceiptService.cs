using Reti.Lab.FoodOnKontainers.Payments.Api.Payment;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Reti.Lab.FoodOnKontainers.Middleware;
using Microsoft.Extensions.Logging;
using System.Net;
using Microsoft.Extensions.DependencyInjection;

namespace Reti.Lab.FoodOnKontainers.Payments.Api.BackgroundService
{
    public class ReceiptService : IHostedService, IDisposable
    {
        private readonly ILogService _logger;
        public IServiceScopeFactory _serviceScopeFactory;

        public ReceiptService(ILogService logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }       

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.Log("Consume Scoped Service Hosted Service is starting.", LogLevel.Information, HttpStatusCode.OK, "CheckReceipt");

            var scope = _serviceScopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IPaymentService>();

            while (!cancellationToken.IsCancellationRequested)
            {
                await service.CheckReceipt();
                await Task.Delay(10000, cancellationToken);
            }
        }

        public interface IScoped { }

        public class Scoped : IScoped { }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            //log
            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }
    }
}
