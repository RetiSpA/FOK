using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.NotificationHub.DTO
{
    public class DeviceRegistration
    {
        public string Platform { get; set; }
        public string Handle { get; set; }
        public string[] Tags { get; set; }
        public string riderName { get; set; }
 
    }
}
