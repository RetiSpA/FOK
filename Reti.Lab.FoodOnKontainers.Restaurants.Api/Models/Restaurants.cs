using GeoAPI.Geometries;
using Newtonsoft.Json;
using Reti.Lab.FoodOnKontainers.DTO.Common;
using System.Collections.Generic;

namespace Reti.Lab.FoodOnKontainers.Restaurants.Api.Models
{
    public partial class Restaurants
    {
        public Restaurants()
        {
            RestaurantsMenu = new HashSet<RestaurantsMenu>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        [JsonIgnore]
        public IGeometry Position { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int IdRestaurantType { get; set; }        
        public bool? Enabled { get; set; }
        public decimal? AverageRating { get; set; }

        public RestaurantTypes IdRestaurantTypeNavigation { get; set; }
        public ICollection<RestaurantsMenu> RestaurantsMenu { get; set; }

        public Position PositionCoordinates
        {
            get
            {
                return Position != null
                    ? new Position()
                    {
                        Latitude = Position.Coordinate.Y,
                        Longitude = Position.Coordinate.X
                    }
                    : null;
            }
        }
    }
}
