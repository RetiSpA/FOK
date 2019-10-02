namespace Reti.Lab.FoodOnKontainers.Events.DTO.Orders
{
    public class OrderRejectedEvent
    {
        public int idOrder { get; set; }
        public int idUser { get; set; }
        public decimal price { get; set; }
    }
}
