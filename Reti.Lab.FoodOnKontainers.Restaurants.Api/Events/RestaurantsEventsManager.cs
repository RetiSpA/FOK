using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Reti.Lab.FoodOnKontainers.Events.DTO;
using Reti.Lab.FoodOnKontainers.Events.DTO.Restaurants;
using Reti.Lab.FoodOnKontainers.Events.DTO.Utilities;
using Reti.Lab.FoodOnKontainers.Middleware;
using Reti.Lab.FoodOnKontainers.Middleware.Dto;
using Reti.Lab.FoodOnKontainers.Restaurants.Api.Services;
using System;
using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.Restaurants.Api.Events
{

    public interface IRestaurantsEventsManager
    {
        void ProductPriceChanged(PriceChangedEvent priceChanged);
        void ProductAvailabilityChanged(ProductAvailabilityChangedEvent productAvailabilityChanged);
    }

    public class RestaurantsEventsManager : IRestaurantsEventsManager
    {
        private ConnectionFactory _factory;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ILogService _logger;

        public RestaurantsEventsManager([FromServices]RabbitMQConfigurations rabbitMQConfiguration, IServiceScopeFactory serviceScopeFactory, ILogService logger)
        {
            Console.WriteLine($"{nameof(RestaurantsEventsManager)} initialized");
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

                _channel.QueueDeclare(queue: ApplicationEvents.BasketQueue,
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);
                _channel.QueueDeclare(queue: ApplicationEvents.RestaurantQueue, durable: true, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += Restaurant_Received;
                _channel.BasicConsume(ApplicationEvents.RestaurantQueue, true, consumer);
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, Microsoft.Extensions.Logging.LogLevel.Error, System.Net.HttpStatusCode.InternalServerError, nameof(RestaurantsEventsManager));
                throw ex;
            }
        }

        #region received events
        private async void Restaurant_Received(object sender, BasicDeliverEventArgs e)
        {
            Console.WriteLine($"A message of type {e.BasicProperties?.Type} has been received received");

            switch (e?.BasicProperties.Type)
            {
                case nameof(RatingChangedEvent):
                    await HandleRatingEvent(MessageSerializationHelper.DeserializeObjectFromBin<RatingChangedEvent>(e.Body));
                    break;
                default:
                    throw new NotImplementedException($"Event not supported: {e.BasicProperties?.Type}");
            }
        }

        private async Task HandleRatingEvent(RatingChangedEvent ratingChangedEvent)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                IRestaurantService restaurantService = scope.ServiceProvider.GetRequiredService<IRestaurantService>();
                await restaurantService.UpdateRestaurantAverageRating(ratingChangedEvent.idRestaurant, ratingChangedEvent.averageRating);
            }
        }

        #endregion

        #region published events

        public void ProductAvailabilityChanged(ProductAvailabilityChangedEvent productAvailabilityChanged)
        {
            IBasicProperties props = _channel.CreateBasicProperties();
            props.Type = nameof(ProductAvailabilityChangedEvent);

            var body = MessageSerializationHelper.SerializeObjectToBin(productAvailabilityChanged);

            _channel.BasicPublish(exchange: string.Empty,
                                 routingKey: ApplicationEvents.BasketQueue,
                                 basicProperties: props,
                                 body: body);
        }

        public void ProductPriceChanged(PriceChangedEvent priceChanged)
        {
            IBasicProperties props = _channel.CreateBasicProperties();
            props.Type = nameof(PriceChangedEvent);

            var body = MessageSerializationHelper.SerializeObjectToBin(priceChanged);

            _channel.BasicPublish(exchange: string.Empty,
                                 routingKey: ApplicationEvents.BasketQueue,
                                 basicProperties: props,
                                 body: body);
        }
        #endregion
    }
}
