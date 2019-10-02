using Microsoft.EntityFrameworkCore;
using Reti.Lab.FoodOnKontainers.Deliveries.Api.DAL;
using Reti.Lab.FoodOnKontainers.Deliveries.Api.Events;
using Reti.Lab.FoodOnKontainers.Deliveries.Api.Mappers;
using Reti.Lab.FoodOnKontainers.Deliveries.Api.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.Deliveries.Api.Services
{
    public class DeliveryService : IDeliveryService
    {
        private readonly DeliveriesDbContext deliveriesDbContext;
        private readonly IDeliveriesEventManager eventManager;

        public DeliveryService(DeliveriesDbContext deliveriesDbContext, IDeliveriesEventManager eventManager)
        {
            this.deliveriesDbContext = deliveriesDbContext;
            this.eventManager = eventManager;
        }

        public async Task<List<Models.Delivery>> GetDeliveries(DTO.DeliveryFilter filter)
        {
            return await deliveriesDbContext.Delivery
                .Where(o => (filter.Today && o.DeliveryRequestedDate.Date == DateTime.Today) || !filter.Today)
                .Where(o => (filter.IdRider.HasValue && filter.IdRider.Value == o.IdRider) || !filter.IdRider.HasValue)
                .Where(o => (filter.Status.HasValue && (int)filter.Status.Value == o.IdStatus) || !filter.Status.HasValue)
                .ToListAsync();
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

        public async Task<int> AddDelivery(DTO.Delivery delivery)
        {
            Models.Delivery newDelivery = DeliveriesMapper.MapNewDeliveryDTO(delivery);
            deliveriesDbContext.Delivery.Add(newDelivery);
            await deliveriesDbContext.SaveChangesAsync();
            return newDelivery.Id;
        }

        public async Task UpdateDelivery(DTO.Delivery delivery)
        {
            Models.Delivery current = await GetDelivery(delivery.Id);
            DeliveriesMapper.MapDeliveryDTO(current, delivery);
            await deliveriesDbContext.SaveChangesAsync();
        }

        public async Task UpdateDeliveryStatus(int idDelivery, DTO.Status newStatus)
        {
            Models.Delivery delivery = await GetDelivery(idDelivery);
            delivery.IdStatus = (int)newStatus;            
            switch (newStatus)
            {
                case DTO.Status.ToPickUp:
                    delivery.TakeChargeDate = DateTime.UtcNow;
                    break;
                case DTO.Status.PickedUp:
                    delivery.PickUpDate = DateTime.UtcNow;
                    break;
                case DTO.Status.Delivered:
                    delivery.DeliveryDate = DateTime.UtcNow;
                    break;
                case DTO.Status.Canceled:
                default:
                    break;
            }
            await deliveriesDbContext.SaveChangesAsync();

            PublishDeliveryStatusChange(delivery);
        }
        
        private void PublishDeliveryStatusChange(Models.Delivery delivery)
        {
            switch ((DTO.Status)delivery.IdStatus)
            {
                case DTO.Status.PickedUp:
                    var deliveryPickedUpMessage = new FoodOnKontainers.Events.DTO.Delivery.DeliveryPickedUpEvent()
                    {
                        idOrder = delivery.IdOrder
                    };
                    eventManager.DeliveryPickedUp(deliveryPickedUpMessage);
                    break;
                case DTO.Status.Delivered:
                    var deliveryCompletedMessage = new FoodOnKontainers.Events.DTO.Delivery.DeliveryCompletedEvent()
                    {
                        idOrder = delivery.IdOrder
                    };
                    eventManager.DeliveryCompleted(deliveryCompletedMessage);
                    break;
                case DTO.Status.ToPickUp:
                case DTO.Status.Canceled:
                default:
                    break;
            }
        }
    }
}
