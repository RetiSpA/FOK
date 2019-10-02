using GeoAPI.Geometries;
using Newtonsoft.Json;
using Reti.Lab.FoodOnKontainers.DTO.Common;
using System;
using System.Collections.Generic;

namespace Reti.Lab.FoodOnKontainers.Orders.Api.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderItem = new HashSet<OrderItem>();
        }

        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public int IdRestaurant { get; set; }
        public string RestaurantName { get; set; }
        public int IdUser { get; set; }
        public string UserName { get; set; }
        public int IdStatus { get; set; }
        public decimal Price { get; set; }
        public string RestaurantAddress { get; set; }
        [JsonIgnore]
        public IGeometry RestaurantPosition { get; set; }
        public string DeliveryAddress { get; set; }
        [JsonIgnore]
        public IGeometry DeliveryPosition { get; set; }
        public DateTime DeliveryRequestedDate { get; set; }

        public Status IdStatusNavigation { get; set; }
        public ICollection<OrderItem> OrderItem { get; set; }

        public Position RestaurantPositionCoordinates
        {
            get
            {
                return RestaurantPosition != null
                    ? new Position()
                    {
                        Latitude = RestaurantPosition.Coordinate.Y,
                        Longitude = RestaurantPosition.Coordinate.X
                    }
                    : null;
            }
        }

        public Position DeliveryPositionCoordinates
        {
            get
            {
                return DeliveryPosition != null
                    ? new Position()
                    {
                        Latitude = DeliveryPosition.Coordinate.Y,
                        Longitude = DeliveryPosition.Coordinate.X
                    }
                    : null;
            }
        }
    }
}
