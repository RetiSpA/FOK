using Reti.Lab.FoodOnKontainers.DTO.Orders;
using System;
using System.Collections.Generic;

namespace Reti.Lab.FoodOnKontainers.Payments.Api.Dto
{
    public class Receipt
    {
        public DateTime Date { get; set; }
        public string RestaurantName { get; set; }        
        public string RestaurantAddress { get; set; }
        public decimal TotalPrice { get; set; }
        public ICollection<OrderItem> OrderItem { get; set; }
    }
}
