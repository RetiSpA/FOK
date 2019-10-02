using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using Reti.Lab.FoodOnKontainers.Events.DTO;
using Reti.Lab.FoodOnKontainers.Events.DTO.Restaurants;
using Reti.Lab.FoodOnKontainers.Events.DTO.Reviews;
using Reti.Lab.FoodOnKontainers.Events.DTO.Utilities;
using Reti.Lab.FoodOnKontainers.Middleware;
using Reti.Lab.FoodOnKontainers.Middleware.Dto;
using Reti.Lab.FoodOnKontainers.Reviews.Api.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.Reviews.Api.Events
{

    public interface IReviewEventsManager
    {
        Task RestaurantRatingChanged(int idRestaurant, decimal newRating);
        Task RiderRatingChanged(int idRider, decimal newRating);
    }

    public class ReviewEventsManager : IReviewEventsManager
    {
        private readonly ConnectionFactory _factory;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ILogService _logger;


        public ReviewEventsManager([FromServices]RabbitMQConfigurations rabbitMQConfiguration, IServiceScopeFactory serviceScopeFactory, ILogService logger)
        {
            Console.WriteLine($"{nameof(ReviewEventsManager)} initialized");
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
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, Microsoft.Extensions.Logging.LogLevel.Error, System.Net.HttpStatusCode.InternalServerError, nameof(ReviewEventsManager));
                throw ex;
            }
        }

        public async Task RestaurantRatingChanged(int idRestaurant, decimal newRating)
        {
            var newRatingEvt = new RatingChangedEvent
            {
                idRestaurant = idRestaurant
            };

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                IReviewService reviewService = scope.ServiceProvider.GetRequiredService<IReviewService>();

                var restaurantReviews = await reviewService.GetRestaurantReviews(idRestaurant);
                newRatingEvt.averageRating = restaurantReviews.Sum(rw => rw.Rating) / restaurantReviews.Count();
            }

            IBasicProperties props = _channel.CreateBasicProperties();
            props.Type = nameof(RatingChangedEvent);

            var body = MessageSerializationHelper.SerializeObjectToBin(newRatingEvt);

            _channel.BasicPublish(exchange: string.Empty,
                                 routingKey: ApplicationEvents.RestaurantQueue,
                                 basicProperties: props,
                                 body: body);
        }

        public async Task RiderRatingChanged(int idRider, decimal newRating)
        {
            var newRatingEvt = new RiderRatingChangedEvent
            {
                IdRider = idRider
            };

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                IReviewService reviewService = scope.ServiceProvider.GetRequiredService<IReviewService>();

                var riderReviews = await reviewService.GetRiderReviews(idRider);
                newRatingEvt.newAverageRating = riderReviews.Sum(rw => rw.Rating) / riderReviews.Count();
            }

            IBasicProperties props = _channel.CreateBasicProperties();
            props.Type = nameof(RiderRatingChangedEvent);

            var body = MessageSerializationHelper.SerializeObjectToBin(newRatingEvt);

            _channel.BasicPublish(exchange: string.Empty,
                                 routingKey: ApplicationEvents.DeliveryQueue,
                                 basicProperties: props,
                                 body: body);
        }
    }
}
