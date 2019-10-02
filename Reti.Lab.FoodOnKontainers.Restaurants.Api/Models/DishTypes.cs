using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Reti.Lab.FoodOnKontainers.Restaurants.Api.Models
{
    public partial class DishTypes
    {
        public DishTypes()
        {
            RestaurantsMenu = new HashSet<RestaurantsMenu>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public ICollection<RestaurantsMenu> RestaurantsMenu { get; set; }
    }
}
