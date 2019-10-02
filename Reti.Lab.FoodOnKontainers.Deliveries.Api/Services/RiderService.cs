using GeoAPI.Geometries;
using Microsoft.EntityFrameworkCore;
using Reti.Lab.FoodOnKontainers.Deliveries.Api.DAL;
using Reti.Lab.FoodOnKontainers.Deliveries.Api.Mappers;
using Reti.Lab.FoodOnKontainers.Deliveries.Api.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.Deliveries.Api.Services
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

        public async Task<Models.Rider> GetRider(int idRider)
        {
            return await deliveriesDbContext.Rider
                .SingleAsync(r => idRider == r.IdRider);
        }

        public async Task<int> AddRider(DTO.Rider rider)
        {
            Models.Rider newRider = RiderMapper.MapNewRiderDTO(rider);
            deliveriesDbContext.Rider.Add(newRider);
            await deliveriesDbContext.SaveChangesAsync();
            return newRider.IdRider;
        }

        public async Task UpdateRider(DTO.Rider rider)
        {
            Models.Rider current = await GetRider(rider.IdRider);
            RiderMapper.MapRiderDTO(current, rider);
            await deliveriesDbContext.SaveChangesAsync();
        }

    }
}
