using GeoAPI.Geometries;
using NetTopologySuite;
using Reti.Lab.FoodOnKontainers.DTO.Common;
using Reti.Lab.FoodOnKontainers.Events.DTO.Orders;
using System;

namespace Reti.Lab.FoodOnKontainers.Deliveries.Api.Mappers
{
    internal class DeliveriesMapper
    {
        internal static Models.Delivery MapNewDeliveryDTO(DTO.Delivery input)
        {
            Models.Delivery output = new Models.Delivery()
            {
                DeliveryAddress = input.DeliveryAddress,
                DeliveryPosition = GetGeometry(input.DeliveryPosition),
                DeliveryName = input.DeliveryName,
                DeliveryRequestedDate = input.DeliveryRequestedDate ?? DateTime.UtcNow.AddHours(1),
                IdOrder = input.IdOrder,
                IdRestaurant = input.IdRestaurant,
                IdStatus = (int)DTO.Status.ToPickUp,
                PickUpAddress = input.PickUpAddress,
                PickUpPosition = GetGeometry(input.PickUpPosition),
                RestaurantName = input.RestaurantName
            };
            return output;
        }

        internal static void MapDeliveryDTO(Models.Delivery current, DTO.Delivery input)
        {
            current.DeliveryAddress = string.IsNullOrEmpty(input.DeliveryAddress) ? current.DeliveryAddress : input.DeliveryAddress;
            current.DeliveryPosition = GetGeometry(input.DeliveryPosition) ?? current.DeliveryPosition;
            current.DeliveryDate = input.DeliveryDate ?? current.DeliveryDate;
            current.DeliveryName = string.IsNullOrEmpty(input.DeliveryName) ? current.DeliveryName : input.DeliveryName;
            current.DeliveryRequestedDate = input.DeliveryRequestedDate ?? current.DeliveryRequestedDate;
            //current.IdOrder = input.IdOrder;
            current.IdRestaurant = input.IdRestaurant ?? current.IdRestaurant;
            current.IdRider = input.IdRider ?? current.IdRider;
            current.IdStatus = input.IdStatus ?? (int)DTO.Status.ToPickUp;
            current.PickUpAddress = string.IsNullOrEmpty(input.PickUpAddress) ? current.PickUpAddress : input.PickUpAddress;
            current.PickUpPosition = GetGeometry(input.PickUpPosition) ?? current.PickUpPosition;
            current.PickUpDate = input.PickUpDate ?? current.PickUpDate;
            current.RestaurantName = string.IsNullOrEmpty(input.RestaurantName) ? current.RestaurantName : input.RestaurantName;
            current.TakeChargeDate = input.TakeChargeDate ?? input.TakeChargeDate;
        }

        internal static DTO.Delivery MapNewDeliveryEvent(OrderAcceptedEvent orderAccepted)
        {
            DTO.Delivery output = new DTO.Delivery()
            {
                DeliveryAddress = orderAccepted.deliveryAddress,
                DeliveryPosition = orderAccepted.deliveryPosition,
                DeliveryName = orderAccepted.deliveryName,
                DeliveryRequestedDate = orderAccepted.deliveryRequestedDate,
                IdOrder = orderAccepted.idOrder,
                IdRestaurant = orderAccepted.idRestaurant,
                IdStatus = (int)DTO.Status.ToPickUp,
                PickUpAddress = orderAccepted.restaurantAddress,
                PickUpPosition = orderAccepted.restaurantPosition,
                RestaurantName = orderAccepted.restaurantName
            };
            return output;
        }

        private static IGeometry GetGeometry(Position position)
        {
            if (position != null)
            {
                var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
                return geometryFactory.CreatePoint(new Coordinate(position.Longitude, position.Latitude));
            }
            return null;
        }
    }
}
