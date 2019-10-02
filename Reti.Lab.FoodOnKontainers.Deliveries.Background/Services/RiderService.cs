using GeoAPI.Geometries;
using Microsoft.EntityFrameworkCore;
using Reti.Lab.FoodOnKontainers.Deliveries.Background.DAL;
using Reti.Lab.FoodOnKontainers.Deliveries.Background.Mappers;
using Reti.Lab.FoodOnKontainers.Deliveries.Background.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.Deliveries.Background.Services
{
    public class RiderService : IRiderService
    {
        private readonly DeliveriesDbContext deliveriesDbContext;

        public RiderService(DeliveriesDbContext deliveriesDbContext)
        {
            this.deliveriesDbContext = deliveriesDbContext;
        }

        public async Task<List<Models.Rider>> GetRiders(DTO.RiderFilter filter)
        {           
            var result = await deliveriesDbContext.Rider
                .Where(r => (filter.Active.HasValue && filter.Active.Value == r.Active) || !filter.Active.HasValue)                
                .ToListAsync();
            
            if (filter.Latitude.HasValue && filter.Longitude.HasValue)
            {
                var pos = new FoodOnKontainers.DTO.Common.Position();
                pos.Latitude = filter.Latitude.Value;
                pos.Longitude = filter.Longitude.Value;
                IGeometry position = RiderMapper.GetGeometry(pos);
                // TODO: verificare significato distanza (gradi)
                result = result
                    .Where(r => r.StartingPoint != null && r.Range.HasValue && position.IsWithinDistance(r.StartingPoint, r.Range.Value))
                    .OrderBy(r => r.StartingPoint.Distance(position))
                    .ToList();
            }            

            return result;
        }
    }
}
