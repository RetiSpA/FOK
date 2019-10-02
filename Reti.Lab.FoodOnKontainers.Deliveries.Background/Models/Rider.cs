using GeoAPI.Geometries;
using Newtonsoft.Json;
using Reti.Lab.FoodOnKontainers.DTO.Common;
using System.Collections.Generic;

namespace Reti.Lab.FoodOnKontainers.Deliveries.Background.Models
{
    public partial class Rider
    {
        public Rider()
        {
            Delivery = new HashSet<Delivery>();
        }

        public int IdRider { get; set; }
        public string RiderName { get; set; }
        public string StartingPointAddress { get; set; }
        [JsonIgnore]
        public IGeometry StartingPoint { get; set; }
        public int? Range { get; set; }
        public decimal? AverageRating { get; set; }
        public bool Active { get; set; }

        public ICollection<Delivery> Delivery { get; set; }

        public Position StartingPointCoordinates
        {
            get
            {
                return StartingPoint != null
                    ? new Position()
                    {
                        Latitude = StartingPoint.Coordinate.Y,
                        Longitude = StartingPoint.Coordinate.X
                    }
                    : null;
            }
        }
    }
}
