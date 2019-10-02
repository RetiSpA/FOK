using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Reti.Lab.FoodOnKontainers.Gateway.Simple.Controllers
{    
    [ApiController]
    public class GatewayController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public GatewayController(
            IHttpClientFactory httpClientFactory
            )
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet]
        [Route("api/[controller]/{service}/[action]")]
        public async Task<ActionResult<IEnumerable<string>>> Values([FromRoute] string service)
        {
            // switch on routes
            string apiRoute = "";
            switch (service)
            {
                case "baskets":
                    apiRoute = "reti-lab-foodonkontainers-basket-api";
                    break;
                case "deliveries":
                    apiRoute = "reti-lab-foodonkontainers-deliveries-api";
                    break;
                case "orders":
                    apiRoute = "reti-lab-foodonkontainers-orders-api";
                    break;
                case "payments":
                    apiRoute = "reti-lab-foodonkontainers-payments-api";
                    break;
                case "restaurants":
                    apiRoute = "reti-lab-foodonkontainers-restaurants-api";
                    break;
                case "reviews":
                    apiRoute = "reti-lab-foodonkontainers-reviews-api";
                    break;
                case "users":
                    apiRoute = "reti-lab-foodonkontainers-users-api";
                    break;
                default:
                    throw new ArgumentException($"Invalid service {service}");
            }
            var response = await CallMicroservice(apiRoute);
            return response.ToList();
        }

        private async Task<IEnumerable<string>> CallMicroservice(string serviceUrl)
        {
            string url = $"http://{serviceUrl}.default.svc.cluster.local/api/values";
            Console.WriteLine($"request url: {url}");
            var res = await _httpClient.GetAsync(url);
            Console.WriteLine($"response: {Newtonsoft.Json.JsonConvert.SerializeObject(res)}");
            return await res.Content.ReadAsAsync<IEnumerable<string>>();
        }
    }
}
