using GeoAPI.Geometries;
using NetTopologySuite;
using Reti.Lab.FoodOnKontainers.DTO.Common;
using Reti.Lab.FoodOnKontainers.Restaurants.Api.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.Restaurants.Api.Mappers
{
    internal class RestaurantsMapper
    {
        internal static Models.Restaurants MapNewRestaurantDTO(RestaurantDTO restaurant)
        {
            var newRestaurant = new Models.Restaurants
            {
                Address = restaurant.Address,
                Position = GetGeometry(restaurant.Position),
                Email = restaurant.Email,
                Enabled = true,
                IdRestaurantType = restaurant.IdRestaurantType,
                Name = restaurant.Name,
                PhoneNumber = restaurant.PhoneNumber
            };

            return newRestaurant;
        }

        internal static Models.Restaurants MapRestaurantDTO(RestaurantDTO restaurant)
        {
            var updatedRestaurant = new Models.Restaurants
            {
                Id = restaurant.Id,
                Address = restaurant.Address,
                Position = GetGeometry(restaurant.Position),
                Email = restaurant.Email,
                Enabled = true,
                IdRestaurantType = restaurant.IdRestaurantType,
                Name = restaurant.Name,
                PhoneNumber = restaurant.PhoneNumber,
                AverageRating = restaurant.AverageRating
            };

            return updatedRestaurant;
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
