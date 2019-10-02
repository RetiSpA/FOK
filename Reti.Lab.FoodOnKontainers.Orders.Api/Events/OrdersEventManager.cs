using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Reti.Lab.FoodOnKontainers.Events.DTO;
using Reti.Lab.FoodOnKontainers.Events.DTO.Delivery;
using Reti.Lab.FoodOnKontainers.Events.DTO.Orders;
using Reti.Lab.FoodOnKontainers.Events.DTO.Utilities;
using Reti.Lab.FoodOnKontainers.Middleware;
using Reti.Lab.FoodOnKontainers.Middleware.Dto;
using Reti.Lab.FoodOnKontainers.Orders.Api.Services;
using System;
using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.Orders.Api.Events
{
    public class OrdersEventManager : IOrdersEventManager
    {
        private readonly ConnectionFactory _factory;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ILogService _logger;

        public OrdersEventManager(IServiceScopeFactory serviceScopeFactory, [FromServices]RabbitMQConfigurations rabbitMQConfiguration, ILogService logger)
        {
            Console.WriteLine($"{nameof(OrdersEventManager)} initialized");

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

                _channel.QueueDeclare(queue: ApplicationEvents.DeliveryQueue, durable: true, exclusive: false, autoDelete: false, arguments: null);
                _channel.QueueDeclare(queue: ApplicationEvents.PaymentQueue, durable: true, exclusive: false, autoDelete: false, arguments: null);
                _channel.QueueDeclare(queue: ApplicationEvents.UserQueue, durable: true, exclusive: false, autoDelete: false, arguments: null);
                _channel.QueueDeclare(queue: ApplicationEvents.OrderQueue, durable: true, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += OrderEvent_Received;
                _channel.BasicConsume(ApplicationEvents.OrderQueue, true, consumer);
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, Microsoft.Extensions.Logging.LogLevel.Error, System.Net.HttpStatusCode.InternalServerError, nameof(OrdersEventManager));
                throw ex;
            }            
        }

        private async void OrderEvent_Received(object sender, BasicDeliverEventArgs e)
        {
            Console.WriteLine($"A message of type {e.BasicProperties?.Type} has been received received");

            try
            {
                switch (e?.BasicProperties.Type)
                {
                    case nameof(DeliveryPickedUpEvent):
                        await HandleStatusChangedEvent(MessageSerializationHelper.DeserializeObjectFromBin<DeliveryPickedUpEvent>(e.Body).idOrder, DTO.Orders.Status.Delivering);
                        break;
                    case nameof(DeliveryCompletedEvent):
                        await HandleStatusChangedEvent(MessageSerializationHelper.DeserializeObjectFromBin<DeliveryCompletedEvent>(e.Body).idOrder, DTO.Orders.Status.Completed);
                        break;
                    default:
                        throw new NotImplementedException($"Event not supported: {e.BasicProperties?.Type}");
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, Microsoft.Extensions.Logging.LogLevel.Error, System.Net.HttpStatusCode.InternalServerError, $"{nameof(OrdersEventManager)} - OrderEvent_Received");
                throw ex;
            }            
        }

        /// <summary>
        /// Pubblica l'evento "Ordine Accettato da gestore" su coda Delivery
        /// </summary>
        /// <param name="orderRejected">Dati del messaggio</param>
        public void OrderAccepted(OrderAcceptedEvent orderAccepted)
        {
            IBasicProperties props = _channel.CreateBasicProperties();
            props.Type = nameof(OrderAcceptedEvent);
            var body = MessageSerializationHelper.SerializeObjectToBin(orderAccepted);

            _channel.BasicPublish(exchange: string.Empty,
                                 routingKey: ApplicationEvents.DeliveryQueue,
                                 basicProperties: props,
                                 body: body);
        }

        /// <summary>
        /// Pubblica l'evento "Ordine Rifiutato da gestore" su coda Payment e User
        /// </summary>
        /// <param name="orderRejected">Dati del messaggio</param>
        public void OrderRejected(OrderRejectedEvent orderRejected)
        {
            IBasicProperties props = _channel.CreateBasicProperties();
            props.Type = nameof(OrderRejectedEvent);
            var body = MessageSerializationHelper.SerializeObjectToBin(orderRejected);

            _channel.BasicPublish(exchange: string.Empty,
                                 routingKey: ApplicationEvents.PaymentQueue,
                                 basicProperties: props,
                                 body: body);

            _channel.BasicPublish(exchange: string.Empty,
                                 routingKey: ApplicationEvents.UserQueue,
                                 basicProperties: props,
                                 body: body);
        }

        private async Task HandleStatusChangedEvent(int idOrder, DTO.Orders.Status status)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                IOrdersService ordersService = scope.ServiceProvider.GetRequiredService<IOrdersService>();
                await ordersService.UpdateStatusOrder(idOrder, status);
            }
        }
    }
}
