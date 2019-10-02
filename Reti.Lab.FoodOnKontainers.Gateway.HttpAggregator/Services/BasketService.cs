using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Reti.Lab.FoodOnKontainers.Gateway.HttpAggregator.Dto;
using System.Net.Http;
using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.Gateway.HttpAggregator
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient _apiClient;
        //private readonly ILogService _logger;
        private readonly UrlsConfig _urls;

        public BasketService(HttpClient httpClient, IOptions<UrlsConfig> config)
        {
            _apiClient = httpClient;
            //_logger = logger;
            _urls = config.Value;
        }

        public async Task<BasketData> GetByIdAsync(int userId)
        {
            var data = await _apiClient.GetStringAsync(_urls.Basket + UrlsConfig.BasketOperations.GetItemById(userId));
            var basket = !string.IsNullOrEmpty(data) ? JsonConvert.DeserializeObject<BasketData>(data) : null;

            return basket;
        }

        public async Task<string> DeleteBasketAsync(int userId)
        {
            var data = await _apiClient.DeleteAsync(_urls.Basket + UrlsConfig.BasketOperations.DeleteBasket(userId));

            return "deleted";
        }
    }
}