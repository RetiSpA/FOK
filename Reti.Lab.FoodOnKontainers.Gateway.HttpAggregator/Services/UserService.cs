using Microsoft.Extensions.Options;
using Reti.Lab.FoodOnKontainers.Middleware;
using System.Net.Http;
using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.Gateway.HttpAggregator
{
    public class UserService : IUserService
    {
        private readonly HttpClient _apiClient;
        //private readonly ILogService _logger;
        private readonly UrlsConfig _urls;

        public UserService(HttpClient httpClient, IOptions<UrlsConfig> config)
        {
            _apiClient = httpClient;
            //_logger = logger;
            _urls = config.Value;
        }

        public async Task<bool> DetractAmount(int userId, decimal amountToDetract)
        {
            var result = await _apiClient.GetAsync(_urls.User + UrlsConfig.UserOperations.DetractAmount(userId, amountToDetract));

            return result.IsSuccessStatusCode;
        }
    }
}
