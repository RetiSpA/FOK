using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Reti.Lab.FoodOnKontainers.Gateway.HttpAggregator.Dto;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.Gateway.HttpAggregator
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _apiClient;
        //private readonly ILogService _logger;
        private readonly UrlsConfig _urls;

        public OrderService(HttpClient httpClient, IOptions<UrlsConfig> config)
        {
            _apiClient = httpClient;
            //_logger = logger;
            _urls = config.Value;
        }

        public async Task<int?> CreateOrderAsync(OrderData order)
        {
            var orderContent = new StringContent(JsonConvert.SerializeObject(order), System.Text.Encoding.UTF8, "application/json");
            var result = await _apiClient.PostAsync(_urls.Order + UrlsConfig.OrdersOperations.CreateOrder(), orderContent);
            if (result.IsSuccessStatusCode && Int32.TryParse(result.Content.ReadAsStringAsync().Result, out int id))
            {               
                return id;
            }
            return null;
        }
    }
}
