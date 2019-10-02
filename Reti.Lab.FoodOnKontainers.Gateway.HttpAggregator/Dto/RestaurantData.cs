namespace Reti.Lab.FoodOnKontainers.Gateway.HttpAggregator.Dto
{
    public class RestaurantData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }        
        public Position PositionCoordinates { get; set; }
    }
}
