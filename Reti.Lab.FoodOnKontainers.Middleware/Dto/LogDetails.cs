using Newtonsoft.Json;

namespace Reti.Lab.FoodOnKontainers.Middleware.Dto
{
    internal class LogDetails
    {
        public LogDetails()
        {
        }

        public string Message { get; set; }
        public string Type { get; set; }
        public string Service { get; set; }
        public string Time { get; set; }
        public int StatusCode { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}