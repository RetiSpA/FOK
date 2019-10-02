using System;
using System.Collections.Generic;
using System.Text;

namespace Reti.Lab.FoodOnKontainers.Events.DTO.Reviews
{
    public class RiderRatingChangedEvent
    {
        public int IdRider { get; set; }
        public decimal newAverageRating { get; set; }

    }
}
