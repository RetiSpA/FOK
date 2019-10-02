using Newtonsoft.Json;

namespace Reti.Lab.FoodOnKontainers.Orders.Api.Models
{
    public partial class OrderItem
    {
        public int IdOrder { get; set; }
        public int IdMenuItem { get; set; }
        public string MenuItemName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        [JsonIgnore]
        public Order IdOrderNavigation { get; set; }
    }
}
