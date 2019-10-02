using Reti.Lab.FoodOnKontainers.DTO.Common;

namespace Reti.Lab.FoodOnKontainers.Deliveries.Api.DTO
{
    public class Rider
    {
        public int IdRider { get; set; }
        public string RiderName { get; set; }
        public string StartingPointAddress { get; set; }
        public Position StartingPoint { get; set; }
        public int? Range { get; set; }
        public decimal? AverageRating { get; set; }
        public bool? Active { get; set; }
    }
}
