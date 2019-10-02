using System;
using System.Collections.Generic;

namespace Reti.Lab.FoodOnKontainers.Reviews.Api.Models
{
    public partial class RiderReview
    {
        public int IdOrder { get; set; }
        public int IdRyder { get; set; }
        public string RyderName { get; set; }
        public int IdUser { get; set; }
        public string UserName { get; set; }
        public string Review { get; set; }
        public short Rating { get; set; }
    }
}
