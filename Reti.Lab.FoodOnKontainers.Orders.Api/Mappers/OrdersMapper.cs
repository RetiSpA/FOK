using GeoAPI.Geometries;
using NetTopologySuite;
using Reti.Lab.FoodOnKontainers.DTO.Common;
using System;
using System.Linq;

namespace Reti.Lab.FoodOnKontainers.Orders.Api.Mappers
{
    internal class OrdersMapper
    {
        internal static Models.Order MapOrderDTO(DTO.Orders.Order input)
        {
            Models.Order output = new Models.Order()
            {
                CreateDate = DateTime.UtcNow,
                DeliveryAddress = input.DeliveryAddress,
                DeliveryPosition = GetGeometry(input.DeliveryPosition),
                DeliveryRequestedDate = input.DeliveryRequestedDate,
                IdRestaurant = input.IdRestaurant,
                IdStatus = (int)DTO.Orders.Status.Inserted,
                IdUser = input.IdUser,
                OrderItem = input.OrderItem
                    .Select(item => new Models.OrderItem()
                    {
                        IdMenuItem = item.IdMenuItem,
                        MenuItemName = item.MenuItemName,
                        Price = item.Price,
                        Quantity = item.Quantity
                    })
                    .ToList(),
                Price = input.Price, // non calcolo somma degli item per permettere eventuali sconti
                RestaurantAddress = input.RestaurantAddress,
                RestaurantPosition = GetGeometry(input.RestaurantPosition),
                RestaurantName = input.RestaurantName,
                UserName = input.UserName
            };
            return output;
        }

        internal static DTO.Orders.Order MapOrderModel(Models.Order orderDb)
        {
            DTO.Orders.Order output = new DTO.Orders.Order()
            {
                Id = orderDb.Id,
                DeliveryAddress = orderDb.DeliveryAddress,
                DeliveryRequestedDate = orderDb.DeliveryRequestedDate,
                DeliveryPosition = orderDb.DeliveryPositionCoordinates,
                IdRestaurant = orderDb.IdRestaurant,
                IdStatus = (DTO.Orders.Status)orderDb.IdStatus,
                IdUser = orderDb.IdUser,
                OrderItem = orderDb.OrderItem
                    .Select(item => new DTO.Orders.OrderItem()
                    {
                        IdMenuItem = item.IdMenuItem,
                        MenuItemName = item.MenuItemName,
                        Price = item.Price,
                        Quantity = item.Quantity
                    })
                    .ToList(),
                Price = orderDb.Price, // non calcolo somma degli item per permettere eventuali sconti
                RestaurantAddress = orderDb.RestaurantAddress,
                RestaurantPosition = orderDb.RestaurantPositionCoordinates,
                RestaurantName = orderDb.RestaurantName,
                UserName = orderDb.UserName
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
