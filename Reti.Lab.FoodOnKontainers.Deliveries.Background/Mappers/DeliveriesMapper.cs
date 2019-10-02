using GeoAPI.Geometries;
using NetTopologySuite;
using Reti.Lab.FoodOnKontainers.DTO.Common;

namespace Reti.Lab.FoodOnKontainers.Deliveries.Background.Mappers
{
    internal class DeliveriesMapper
    {
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
