namespace Reti.Lab.FoodOnKontainers.DTO.Orders
{
    public class OrderFilter
    {
        public bool Today { get; set; }
        public int? IdRestaurant { get; set; }
        public int? IdUser { get; set; }
        public Status? Status { get; set; }        
    }
}
