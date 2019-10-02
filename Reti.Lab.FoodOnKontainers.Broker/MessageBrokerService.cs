using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore;
using Reti.Lab.FoodOnKontainers.Middleware.Dto;

namespace Reti.Lab.FoodOnKontainers.Broker
{
    public class MessageBrokerService : BackgroundService
    {
        private static IConfiguration _configuration;        
        private readonly ILogger _logger;

        public MessageBrokerService(ILogger<MessageBrokerService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;            
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var webHostBuilder = WebHost.CreateDefaultBuilder();

            var rabbitMQConfigurations = new RabbitMQConfigurations();
            new ConfigureFromConfigurationOptions<RabbitMQConfigurations>(
                _configuration.GetSection("RabbitMQConfigurations"))
                        .Configure(rabbitMQConfigurations);

            var mongoConfigurations = new MongoDBConfigurations();
            new ConfigureFromConfigurationOptions<MongoDBConfigurations>(
                _configuration.GetSection("MongoDBConfigurations"))
                        .Configure(mongoConfigurations);

            var mongoClient = new MongoClient(string.Format("mongodb://{0}:{1}", mongoConfigurations.Host, mongoConfigurations.Port));
            IMongoDatabase mongoDb = mongoClient.GetDatabase("LogsDB");
            var logsCollection = mongoDb.GetCollection<BsonDocument>("Logs");

            var factory = new ConnectionFactory()
            {
                HostName = rabbitMQConfigurations.HostName,
                Port = rabbitMQConfigurations.Port,
                UserName = rabbitMQConfigurations.UserName,
                Password = rabbitMQConfigurations.Password
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "logQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = System.Text.Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received:\n {0}", message);

                    BsonDocument document = BsonDocument.Parse(message);
                    try
                    {
                        logsCollection.InsertOne(document);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error in mongoDB insert: {ex.Message}");
                    }
                    
                };

                channel.BasicConsume(queue: "logQueue", autoAck: true, consumer: consumer);

                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(5000, stoppingToken);                    
                }
            }
        }
    }
}