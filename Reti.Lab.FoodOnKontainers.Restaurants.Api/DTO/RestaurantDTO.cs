using Reti.Lab.FoodOnKontainers.DTO.Common;

namespace Reti.Lab.FoodOnKontainers.Restaurants.Api.DTO
{
    public class RestaurantDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public Position Position { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int IdRestaurantType { get; set; }        
        public bool? Enabled { get; set; }
        public decimal? AverageRating { get; set; }
    }
}
