using GeoAPI.Geometries;
using NetTopologySuite;
using Reti.Lab.FoodOnKontainers.DTO.Common;

namespace Reti.Lab.FoodOnKontainers.Deliveries.Api.Mappers
{
    internal class RiderMapper
    {
        internal static Models.Rider MapNewRiderDTO(DTO.Rider input)
        {
            Models.Rider output = new Models.Rider()
            {
                Active = true,
                AverageRating = null,
                Range = input.Range,
                RiderName = input.RiderName,
                StartingPointAddress = input.StartingPointAddress,
                StartingPoint = GetGeometry(input.StartingPoint)
            };
            return output;
        }

        internal static void MapRiderDTO(Models.Rider current, DTO.Rider input)
        {
            current.Active = input.Active ?? current.Active;
            current.AverageRating = input.AverageRating ?? current.AverageRating;
            current.Range = input.Range ?? current.Range;
            current.RiderName = string.IsNullOrEmpty(input.RiderName) ? current.RiderName : input.RiderName;
            current.StartingPointAddress = string.IsNullOrEmpty(input.StartingPointAddress) ? current.StartingPointAddress : input.StartingPointAddress;
            current.StartingPoint = GetGeometry(input.StartingPoint) ?? current.StartingPoint;
        }

        internal static IGeometry GetGeometry(Position position)
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
