using System.Collections.Generic;

namespace Reti.Lab.FoodOnKontainers.Gateway.HttpAggregator.Dto
{
    public class BasketData
    {
        public int UserId { get; set; }
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }

        public List<UserBasketItem> BasketItems { get; set; }
    }

    public class UserBasketItem
    {
        public int MenuItemId { get; set; }
        public string MenuItemName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public bool Available { get; set; } = true;
    }
}
