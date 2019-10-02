using Reti.Lab.FoodOnKontainers.DTO.Common;

namespace Reti.Lab.FoodOnKontainers.Deliveries.Api.DTO
{
    public class RiderFilter
    {
        public bool? Active { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
    }
}
