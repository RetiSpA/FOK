using System;
using System.Collections.Generic;
using System.Text;

namespace Reti.Lab.FoodOnKontainers.Events.DTO.Restaurants
{
    public class RatingChangedEvent
    {
         public int idRestaurant { get; set; }
        public decimal averageRating { get; set; }
    }
}
