using System;

namespace Reti.Lab.FoodOnKontainers.Gateway.HttpAggregator.Dto
{
    public class ConfirmBasketDto
    {
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string DeliveryAddress { get; set; }
        public Position DeliveryPosition { get; set; }
        public DateTime DeliveryRequestedDate { get; set; }
    }

   
}
