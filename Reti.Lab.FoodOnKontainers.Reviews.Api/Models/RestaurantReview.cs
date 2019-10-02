using System;
using System.Collections.Generic;

namespace Reti.Lab.FoodOnKontainers.Reviews.Api.Models
{
    public partial class RestaurantReview
    {
        public int IdOrder { get; set; }
        public int IdRestaurant { get; set; }
        public string RestaurantName { get; set; }
        public int IdUser { get; set; }
        public string UserName { get; set; }
        public string Review { get; set; }
        public short Rating { get; set; }
    }
}
