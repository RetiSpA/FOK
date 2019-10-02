using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Reti.Lab.FoodOnKontainers.Gateway.HttpAggregator.Dto;
using System.Net.Http;
using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.Gateway.HttpAggregator
{
    public class RestaurantService : IRestaurantService
    {
        private readonly HttpClient _apiClient;
        private readonly UrlsConfig _urls;

        public RestaurantService(HttpClient httpClient, IOptions<UrlsConfig> config)
        {
            _apiClient = httpClient;
            _urls = config.Value;
        }

        public async Task<RestaurantData> GetRestaurantAsync(int restaurantId)
        {
            var data = await _apiClient.GetStringAsync(_urls.Restaurant + UrlsConfig.RestaurantsOperations.GetRestaurantById(restaurantId));
            var restaurant = !string.IsNullOrEmpty(data) ? JsonConvert.DeserializeObject<RestaurantData>(data) : null;
            return restaurant;
        }
    }
}
