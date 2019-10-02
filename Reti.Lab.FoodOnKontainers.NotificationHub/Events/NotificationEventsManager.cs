using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Reti.Lab.FoodOnKontainers.Events.DTO;
using Reti.Lab.FoodOnKontainers.Events.DTO.Delivery;
using Reti.Lab.FoodOnKontainers.Events.DTO.Utilities;
using Reti.Lab.FoodOnKontainers.Middleware;
using Reti.Lab.FoodOnKontainers.Middleware.Dto;
using Reti.Lab.FoodOnKontainers.NotificationHub.PushNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.NotificationHub.Events
{
    interface INotificationEventManager { }

    public class NotificationEventsManager : INotificationEventManager
    {
        private readonly ConnectionFactory _factory;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ILogService _logger;
        private readonly Notifications notificationHubInstance;

        public NotificationEventsManager(IServiceScopeFactory serviceScopeFactory, 
                                        [FromServices]RabbitMQConfigurations rabbitMQConfiguration, 
                                        ILogService logger,
                                        Notifications notificationHub)
        {

            Console.WriteLine($"{nameof(NotificationEventsManager)} initialized");
            _logger = logger;

            try
            {
                _serviceScopeFactory = serviceScopeFactory;
                notificationHubInstance = notificationHub;

                _factory = new ConnectionFactory()
                {
                    HostName = rabbitMQConfiguration.HostName,
                    Port = rabbitMQConfiguration.Port,
                    UserName = rabbitMQConfiguration.UserName,
                    Password = rabbitMQConfiguration.Password
                };

                _connection = _factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.QueueDeclare(queue: ApplicationEvents.pushNotificationQueue, durable: true, exclusive: false, autoDelete: false, arguments: null);
               

                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += Notification_Received;
                _channel.BasicConsume(ApplicationEvents.pushNotificationQueue, true, consumer);
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, Microsoft.Extensions.Logging.LogLevel.Error, System.Net.HttpStatusCode.InternalServerError, nameof(NotificationEventsManager));
                throw ex;
            }

        }

        private async void Notification_Received(object sender, BasicDeliverEventArgs e)
        {

            Console.WriteLine($"A message of type {e.BasicProperties?.Type} has been received received");

            switch (e?.BasicProperties.Type)
            {
                case nameof(DeliveryReadyEvent):
                    await HandleDeliveryReadyEvent(MessageSerializationHelper.DeserializeObjectFromBin<DeliveryReadyEvent>(e.Body));
                    break;
                default:
                    throw new NotImplementedException($"Event not supported: {e.BasicProperties?.Type}");
            }
        }

        private async Task HandleDeliveryReadyEvent(DeliveryReadyEvent deliverReady)
        {
            string[] userTag = new string[2];
            userTag[0] = "rider:" + deliverReady.ryderId;
          

            Microsoft.Azure.NotificationHubs.NotificationOutcome outcome = null;

            switch (deliverReady.pns.ToLower())
            {
                case "apns":
                    // iOS
                    var alert = "{\"aps\":{\"alert\":\"New Delivery is ready\"}}";
                    outcome = await notificationHubInstance.Hub.SendAppleNativeNotificationAsync(alert, userTag);
                    break;
                case "fcm":
                    // Android
                    var notif = "{ \"data\" : {\"message\":\"New Delivery is ready\",\"deliveryId\":\""+deliverReady.deliveryId+"\"}}";
                    outcome = await notificationHubInstance.Hub.SendFcmNativeNotificationAsync(notif, userTag);
                    break;
            }

            if (outcome != null)
            {
                if (!((outcome.State == Microsoft.Azure.NotificationHubs.NotificationOutcomeState.Abandoned) ||
                    (outcome.State == Microsoft.Azure.NotificationHubs.NotificationOutcomeState.Unknown)))
                {

                }
            }
        }
    }
}
