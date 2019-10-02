using Microsoft.EntityFrameworkCore;
using Reti.Lab.FoodOnKontainers.Deliveries.Background.DAL;
using Reti.Lab.FoodOnKontainers.Deliveries.Background.Mappers;
using Reti.Lab.FoodOnKontainers.Deliveries.Background.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.Deliveries.Background.Services
{
    public class DeliveryService : IDeliveryService
    {
        private readonly DeliveriesDbContext deliveriesDbContext;

        public DeliveryService(DeliveriesDbContext deliveriesDbContext)
        {
            this.deliveriesDbContext = deliveriesDbContext;
        }

        public IEnumerable<Models.Delivery> GetDeliveriesToAssign()
        {
            return deliveriesDbContext.Delivery
                .Where(o => !o.IdRider.HasValue)
                .Where(o => o.IdStatus == (int)DTO.Status.ToPickUp)
                .Where(o => o.DeliveryRequestedDate.Date == DateTime.Today)
                .Where(o => !o.TakeChargeDate.HasValue)
                .OrderBy(o => o.DeliveryRequestedDate);
        }

        public async Task<Models.Delivery> GetDelivery(int idDelivery)
        {
            return await deliveriesDbContext.Delivery
                .Include(o => o.IdRiderNavigation)
                .Include(o => o.IdStatusNavigation)
                .SingleAsync(o => idDelivery == o.Id);
        }

        public async Task UpdateDelivery(DTO.Delivery delivery)
        {
            Models.Delivery current = await GetDelivery(delivery.Id);
            DeliveriesMapper.MapDeliveryDTO(current, delivery);
            await deliveriesDbContext.SaveChangesAsync();
        }
    
    }
}
