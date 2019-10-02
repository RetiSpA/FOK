using System;
using System.Collections.Generic;
using System.Text;

namespace Reti.Lab.FoodOnKontainers.Events.DTO.Delivery
{
    public class DeliveryReadyEvent
    {
        public int ryderId;
        public string ryderDevice;
        public int deliveryId;
        public string pns = "fcm";
    }
}
