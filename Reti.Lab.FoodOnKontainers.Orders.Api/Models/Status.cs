using Newtonsoft.Json;
using System.Collections.Generic;

namespace Reti.Lab.FoodOnKontainers.Orders.Api.Models
{
    public partial class Status
    {
        public Status()
        {
            Order = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public virtual ICollection<Order> Order { get; set; }
    }
}
