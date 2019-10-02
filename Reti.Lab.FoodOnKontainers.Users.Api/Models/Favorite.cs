using System;
using System.Collections.Generic;

namespace Reti.Lab.FoodOnKontainers.Users.Api.Models
{
    public partial class Favorite
    {
        public int RestaurantId { get; set; }
        public int UserId { get; set; }
    }
}
