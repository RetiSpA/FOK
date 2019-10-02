using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Reti.Lab.FoodOnKontainers.Gateway.HttpAggregator.Dto;
using System.Net.Http;
using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.Gateway.HttpAggregator
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _apiClient;
        //private readonly ILogService _logger;
        private readonly UrlsConfig _urls;

        public PaymentService(HttpClient httpClient, IOptions<UrlsConfig> config)
        {
            _apiClient = httpClient;
            //_logger = logger;
            _urls = config.Value;
        }

        public async Task<bool> CreateTransactionAsync(TransactionData transaction)
        {            
            var transactionContent = new StringContent(JsonConvert.SerializeObject(transaction), System.Text.Encoding.UTF8, "application/json");

            var result = await _apiClient.PostAsync(_urls.Payment + UrlsConfig.PaymentOperations.CreateTransaction(), transactionContent);

            return result.IsSuccessStatusCode;
        }
    }
}
