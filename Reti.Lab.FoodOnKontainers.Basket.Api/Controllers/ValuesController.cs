using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Reti.Lab.FoodOnKontainers.Basket.Api.Settings;

namespace Reti.Lab.FoodOnKontainers.Basket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private RedisConfig _redisConfig;

        public ValuesController(IOptions<RedisConfig> redisConfig) => _redisConfig = redisConfig.Value;
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "Hello from BASKETS microservice!!!" };
        }

        [HttpGet("env")]
        public async Task<IActionResult> GetEnvironment()
        {
            string envInfo = $"{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")} - redis: {_redisConfig.connection}";
           
            return Ok(envInfo);
        }

    }
}
