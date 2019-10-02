using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.Reviews.Api.DTO
{
    public class RestaurantReviewDto
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
