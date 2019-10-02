using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.Basket.Api.Dto
{
    public class UserBasketItem
    {
        public int menuItemId { get; set; }
        public string menuItemName { get; set; }
        public int quantity { get; set; }
        public decimal price { get; set; }
        public bool available { get; set; } = true;
    }
}
