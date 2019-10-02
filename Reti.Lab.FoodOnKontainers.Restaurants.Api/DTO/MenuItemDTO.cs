using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.Restaurants.Api.DTO
{
    public class MenuItemDTO
    {
        public int Id { get; set; }
        public int IdRestaurant { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int IdDishType { get; set; }
        public decimal? Price { get; set; }
        public decimal? Promo { get; set; }
    }
}
