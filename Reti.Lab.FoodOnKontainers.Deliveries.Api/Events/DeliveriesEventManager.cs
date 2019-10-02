using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Reti.Lab.FoodOnKontainers.Deliveries.Api.DTO;
using Reti.Lab.FoodOnKontainers.Deliveries.Api.Mappers;
using Reti.Lab.FoodOnKontainers.Deliveries.Api.Services.Interfaces;
using Reti.Lab.FoodOnKontainers.Events.DTO;
using Reti.Lab.FoodOnKontainers.Events.DTO.Delivery;
using Reti.Lab.FoodOnKontainers.Events.DTO.Orders;
using Reti.Lab.FoodOnKontainers.Events.DTO.Reviews;
using Reti.Lab.FoodOnKontainers.Events.DTO.Utilities;
using Reti.Lab.FoodOnKontainers.Middleware;
using Reti.Lab.FoodOnKontainers.Middleware.Dto;
using System;
using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.Deliveries.Api.Events
{
    public class DeliveriesEventManager : IDeliveriesEventManager
    {
        private readonly ConnectionFactory _factory;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ILogService _logger;

        public DeliveriesEventManager(IServiceScopeFactory serviceScopeFactory, [FromServices]RabbitMQConfigurations rabbitMQConfiguration, ILogService logger)
        {
            Console.WriteLine($"{nameof(DeliveriesEventManager)} initialized");
            _logger = logger;

            try
            {
                _serviceScopeFactory = serviceScopeFactory;

                _factory = new ConnectionFactory()
                {
                    HostName = rabbitMQConfiguration.HostName,
                    Port = rabbitMQConfiguration.Port,
                    UserName = rabbitMQConfiguration.UserName,
                    Password = rabbitMQConfiguration.Password
                };

                _connection = _factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.QueueDeclare(queue: ApplicationEvents.OrderQueue, durable: true, exclusive: false, autoDelete: false, arguments: null);
                _channel.QueueDeclare(queue: ApplicationEvents.DeliveryQueue, durable: true, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += DeliveryEvent_Received;
                _channel.BasicConsume(ApplicationEvents.DeliveryQueue, true, consumer);
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, Microsoft.Extensions.Logging.LogLevel.Error, System.Net.HttpStatusCode.InternalServerError, nameof(DeliveriesEventManager));
                throw ex;
            }
        }

        private async void DeliveryEvent_Received(object sender, BasicDeliverEventArgs e)
        {
            Console.WriteLine($"A message of type {e.BasicProperties?.Type} has been received received");

            switch (e?.BasicProperties.Type)
            {
                case nameof(OrderAcceptedEvent):
                    HandleOrderAcceptedEvent(MessageSerializationHelper.DeserializeObjectFromBin<OrderAcceptedEvent>(e.Body));
                    break;
                case nameof(RiderRatingChangedEvent):
                    await HandleRiderRatingChangedEvent(MessageSerializationHelper.DeserializeObjectFromBin<RiderRatingChangedEvent>(e.Body));
                    break;
                default:
                    throw new NotImplementedException($"Event not supported: {e.BasicProperties?.Type}");
            }
        }

        /// <summary>
        /// Pubblica l'evento "Presa in consegna da rider" su coda Order
        /// </summary>
        /// <param name="deliveryPickedUp">Dati del messaggio</param>
        public void DeliveryPickedUp(DeliveryPickedUpEvent deliveryPickedUp)
        {
            IBasicProperties props = _channel.CreateBasicProperties();
            props.Type = nameof(DeliveryPickedUpEvent);
            var body = MessageSerializationHelper.SerializeObjectToBin(deliveryPickedUp);

            _channel.BasicPublish(exchange: string.Empty,
                                 routingKey: ApplicationEvents.OrderQueue,
                                 basicProperties: props,
                                 body: body);
        }

        /// <summary>
        /// Pubblica l'evento "Consegna completata da rider" su coda Order
        /// </summary>
        /// <param name="deliveryCompleted">Dati del messaggio</param>
        public void DeliveryCompleted(DeliveryCompletedEvent deliveryCompleted)
        {
            IBasicProperties props = _channel.CreateBasicProperties();
            props.Type = nameof(DeliveryCompletedEvent);
            var body = MessageSerializationHelper.SerializeObjectToBin(deliveryCompleted);

            _channel.BasicPublish(exchange: string.Empty,
                                 routingKey: ApplicationEvents.OrderQueue,
                                 basicProperties: props,
                                 body: body);
        }

        private void HandleOrderAcceptedEvent(OrderAcceptedEvent orderAccepted)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                IDeliveryService deliveryService = scope.ServiceProvider.GetRequiredService<IDeliveryService>();
                deliveryService.AddDelivery(DeliveriesMapper.MapNewDeliveryEvent(orderAccepted));
            }
        }

        private async Task HandleRiderRatingChangedEvent(RiderRatingChangedEvent riderRatingChanged)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                IRiderService riderService = scope.ServiceProvider.GetRequiredService<IRiderService>();

                var updatedRider = new Rider
                {
                    IdRider = riderRatingChanged.IdRider,
                    AverageRating = riderRatingChanged.newAverageRating
                };

                await riderService.UpdateRider(updatedRider);
            }
        }
    }
}
