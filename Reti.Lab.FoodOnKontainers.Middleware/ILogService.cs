using Microsoft.Extensions.Logging;
using System.Net;

namespace Reti.Lab.FoodOnKontainers.Middleware
{
    public interface ILogService
    {
        void Log(string message, LogLevel error, HttpStatusCode statusCode, string serviceName);
    }
}