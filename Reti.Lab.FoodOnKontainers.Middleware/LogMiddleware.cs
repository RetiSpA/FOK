using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Reti.Lab.FoodOnKontainers.Middleware.Dto;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.Middleware
{
    public class LogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IModel _channel;

        public LogMiddleware(RequestDelegate next, [FromServices]Dto.RabbitMQConfigurations configurations)
        {
            _next = next;

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

        public static void AddRabbitMQConfiguration(IServiceCollection services, IConfiguration configuration)
        {
            var rabbitMQConfiguration = new RabbitMQConfigurations();
            new ConfigureFromConfigurationOptions<RabbitMQConfigurations>(
                configuration.GetSection("RabbitMQConfigurations"))
                    .Configure(rabbitMQConfiguration);

            services.AddSingleton(rabbitMQConfiguration);
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                Send(_channel, LogLevel.Error, $"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(new Dto.ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = "Internal Server Error from the custom middleware."
            }.ToString());
        }

        private void Send(IModel channel, LogLevel error, string message)
        {
            if (string.IsNullOrEmpty(message))
                return;

            var errorDetail = new Dto.ErrorDetails()
            {
                Message = message,
                Type = error.ToString(),
                Service = "User service",
                Time = DateTime.Now,
                StatusCode = (int)HttpStatusCode.InternalServerError
            };

            var json = Encoding.UTF8.GetBytes(errorDetail.ToString());

            channel.BasicPublish(exchange: "",
                                 routingKey: "logQueue",
                                 basicProperties: null,
                                 body: json);
        }
    }
}
