using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Reti.Lab.FoodOnKontainers.Deliveries.Background.Services.Interfaces;
using Reti.Lab.FoodOnKontainers.Middleware;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.Deliveries.Background.Services.HostedServices
{
    public class RiderHandlerHostedService : BackgroundService
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly ILogService logger;

        public RiderHandlerHostedService(
            IServiceScopeFactory serviceScopeFactory,
            ILogService logger)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.logger = logger;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine($"I'm {nameof(RiderHandlerHostedService)}");
            while (!stoppingToken.IsCancellationRequested)
            {
                await AssignDeliveryToRider();
                await Task.Delay(10000, stoppingToken);
            }
        }

        private async Task AssignDeliveryToRider()
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                IDeliveryService deliveryService = scope.ServiceProvider.GetRequiredService<IDeliveryService>();
                IRiderService riderService = scope.ServiceProvider.GetRequiredService<IRiderService>();
                var deliveryToProcess = deliveryService.GetDeliveriesToAssign().FirstOrDefault();
                if (deliveryToProcess != null)
                {
                    logger.Log($"Processing delivery {deliveryToProcess.Id}", Microsoft.Extensions.Logging.LogLevel.Information, System.Net.HttpStatusCode.OK, nameof(RiderHandlerHostedService));
                    var availableRiders = await riderService.GetRiders(new DTO.RiderFilter() { Active = true });
                    if (availableRiders.Any())
                    {                       
                        var rider = availableRiders
                            .Where(r => r.StartingPoint != null)
                            // TODO: stiamo usando solo il punto di pick up e non stiamo usando il range...migliorare l'algoritmo :)
                            .OrderBy(r => r.StartingPoint.Distance(deliveryToProcess.PickUpPosition))
                            .First();
                        await deliveryService.UpdateDelivery(new DTO.Delivery()
                        {
                            Id = deliveryToProcess.Id,
                            IdRider = rider.IdRider,
                            TakeChargeDate = DateTime.UtcNow
                        });
                        logger.Log($"Assigned rider {rider.IdRider} - \"{rider.RiderName}\" to delivery {deliveryToProcess.Id}", Microsoft.Extensions.Logging.LogLevel.Information, System.Net.HttpStatusCode.OK, nameof(RiderHandlerHostedService));
                    }
                    else
                    {
                        logger.Log($"No available riders for delivery {deliveryToProcess.Id}", Microsoft.Extensions.Logging.LogLevel.Warning, System.Net.HttpStatusCode.OK, nameof(RiderHandlerHostedService));
                    }
                }
            }            
        }        
    }
}
