using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.Basket.Api.Settings
{
    public class RedisConfig
    {
        public string connection { get; set; }
        public string instanceName { get; set; }
        public int expirationMinutes { get; set; }
    }
}
