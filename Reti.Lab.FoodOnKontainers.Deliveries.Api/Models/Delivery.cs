using GeoAPI.Geometries;
using Newtonsoft.Json;
using Reti.Lab.FoodOnKontainers.DTO.Common;
using System;

namespace Reti.Lab.FoodOnKontainers.Deliveries.Api.Models
{
    public partial class Delivery
    {
        public int Id { get; set; }
        public int IdOrder { get; set; }
        public int? IdRider { get; set; }
        public DateTime? TakeChargeDate { get; set; }
        public DateTime? PickUpDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime DeliveryRequestedDate { get; set; }
        public string PickUpAddress { get; set; }
        [JsonIgnore]
        public IGeometry PickUpPosition { get; set; }
        public string DeliveryAddress { get; set; }
        [JsonIgnore]
        public IGeometry DeliveryPosition { get; set; }
        public int IdStatus { get; set; }
        public int? IdRestaurant { get; set; }
        public string RestaurantName { get; set; }
        public string DeliveryName { get; set; }
        [JsonIgnore]
        public virtual Rider IdRiderNavigation { get; set; }
        public virtual Status IdStatusNavigation { get; set; }

        public Position PickUpPositionCoordinates
        {
            get
            {
                return PickUpPosition != null
                    ? new Position()
                    {
                        Latitude = PickUpPosition.Coordinate.Y,
                        Longitude = PickUpPosition.Coordinate.X
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
