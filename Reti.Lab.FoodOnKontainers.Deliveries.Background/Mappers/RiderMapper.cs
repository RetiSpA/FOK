using GeoAPI.Geometries;
using NetTopologySuite;
using Reti.Lab.FoodOnKontainers.DTO.Common;

namespace Reti.Lab.FoodOnKontainers.Deliveries.Background.Mappers
{
    internal class RiderMapper
    {
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
