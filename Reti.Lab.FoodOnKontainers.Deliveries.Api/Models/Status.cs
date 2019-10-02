using Newtonsoft.Json;
using System.Collections.Generic;

namespace Reti.Lab.FoodOnKontainers.Deliveries.Api.Models
{
    public partial class Status
    {
        public Status()
        {
            Delivery = new HashSet<Delivery>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public virtual ICollection<Delivery> Delivery { get; set; }
    }
}
