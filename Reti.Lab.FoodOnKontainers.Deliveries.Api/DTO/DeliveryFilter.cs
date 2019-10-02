namespace Reti.Lab.FoodOnKontainers.Deliveries.Api.DTO
{
    public class DeliveryFilter
    {
        public bool Today { get; set; }
        public int? IdRider { get; set; }
        public Status? Status { get; set; }
    }
}
