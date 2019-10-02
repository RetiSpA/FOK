using Newtonsoft.Json;
using System;

namespace Reti.Lab.FoodOnKontainers.Middleware.Dto
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public string Service { get; set; }
        public DateTime Time { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
