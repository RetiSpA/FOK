using Newtonsoft.Json;

namespace Reti.Lab.FoodOnKontainers.Restaurants.Api.Models
{
    public partial class RestaurantsMenu
    {
        public int Id { get; set; }
        public int IdRestaurant { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int IdDishType { get; set; }
        public decimal? Price { get; set; }
        public decimal? Promo { get; set; }

        public DishTypes IdDishTypeNavigation { get; set; }
        [JsonIgnore]
        public Restaurants IdRestaurantNavigation { get; set; }
        public virtual MenuPhoto MenuPhoto { get; set; }
    }
}
