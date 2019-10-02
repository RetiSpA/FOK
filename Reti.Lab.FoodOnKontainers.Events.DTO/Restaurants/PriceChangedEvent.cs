using System;
using System.Collections.Generic;
using System.Text;

namespace Reti.Lab.FoodOnKontainers.Events.DTO.Restaurants
{
    public class PriceChangedEvent
    {
        public int restaurantId { get; set; }
        public int itemId { get; set; }
        public decimal newPrice { get; set; }
       
    } 
}
