using Reti.Lab.FoodOnKontainers.Gateway.HttpAggregator.Dto;
using System;
using System.Collections.Generic;

namespace Reti.Lab.FoodOnKontainers.Gateway.HttpAggregator.Dto
{
    public class OrderData
    {
        public int Id { get; set; }
        public int IdRestaurant { get; set; }
        public string RestaurantName { get; set; }
        public int IdUser { get; set; }
        public string UserName { get; set; }
        public decimal Price { get; set; }
        public string RestaurantAddress { get; set; }
        public Position RestaurantPosition { get; set; }
        public string DeliveryAddress { get; set; }
        public Position DeliveryPosition { get; set; }
        public DateTime DeliveryRequestedDate { get; set; }
        public List<OrderItem> OrderItem { get; set; }
    }

    public partial class OrderItem
    {
        public int IdOrder { get; set; }
        public int IdMenuItem { get; set; }
        public string MenuItemName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
