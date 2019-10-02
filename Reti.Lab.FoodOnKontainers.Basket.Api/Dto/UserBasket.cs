using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.Basket.Api.Dto
{
    public class UserBasket
    {
        public int userId { get; set; }
        public int restaurantId { get; set; }
        public string restaurantName { get; set; }
       
        public List<UserBasketItem> basketItems { get; set; }
    }
}
