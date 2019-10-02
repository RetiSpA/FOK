using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.Restaurants.Api.DTO
{
    public class RestaurantRatingDTO
    {
        public int idRestaurant { get; set; }

        public decimal averageRating { get; set; }
    }
}
