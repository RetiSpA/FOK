namespace Reti.Lab.FoodOnKontainers.Events.DTO.Restaurants
{
    public class ProductAvailabilityChangedEvent
    {
        public int restaurantId { get; set; }
        public int itemId { get; set; }
        public bool available { get; set; }
    }
}
