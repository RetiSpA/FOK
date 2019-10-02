using System;
using System.Collections.Generic;

namespace Reti.Lab.FoodOnKontainers.Restaurants.Api.Models
{
    public partial class MenuPhoto
    {
        public int IdMenu { get; set; }
        public byte[] Photo { get; set; }

        public RestaurantsMenu IdMenuNavigation { get; set; }
    }
}
