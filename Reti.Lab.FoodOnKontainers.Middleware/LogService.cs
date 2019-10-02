using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Reti.Lab.FoodOnKontainers.Middleware.Dto;
using System;
using System.Net;
using System.Text;

namespace Reti.Lab.FoodOnKontainers.Middleware
{
    public class LogService : ILogService
    {
        private readonly IModel _channel;

        public LogService([FromServices]Dto.RabbitMQConfigurations configurations)
        {
            var factory = new ConnectionFactory()
            {
                HostName = configurations.HostName,
                Port = configurations.Port,
                UserName = configurations.UserName,
                Password = configurations.Password
            };

            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();

            _channel.QueueDeclare(queue: "logQueue",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);
        }

        public void Log(string message, LogLevel error, HttpStatusCode statusCode, string serviceName)
        {
            string datetimeFormat = "yyyy-MM-dd HH:mm:ss.fff";

            var logDetail = new LogDetails()
            {
                Message = message,
                Type = error.ToString(),
                Service = serviceName,
                Time = DateTime.Now.ToString(datetimeFormat),
                StatusCode = (int)statusCode
            };

            var json = Encoding.UTF8.GetBytes(logDetail.ToString());

            _channel.BasicPublish(exchange: "",
                                 routingKey: "logQueue",
                                 basicProperties: null,
                                 body: json);
        }
    }
}
