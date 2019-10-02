using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.Basket.Api.Dto
{
    public class UserBasketItemToChange
    {
        public int id { get; set; }
        public int itemId { get; set; }

        public int quantity { get; set; }
    }
}
