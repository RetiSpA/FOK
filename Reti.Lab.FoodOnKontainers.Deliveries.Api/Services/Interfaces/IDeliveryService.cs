using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.Deliveries.Api.Services.Interfaces
{
    public interface IDeliveryService
    {
        Task<List<Models.Delivery>> GetDeliveries(DTO.DeliveryFilter filter);
        IEnumerable<Models.Delivery> GetDeliveriesToAssign();
        Task<Models.Delivery> GetDelivery(int idDelivery);
        Task<int> AddDelivery(DTO.Delivery delivery);
        Task UpdateDelivery(DTO.Delivery delivery);
        Task UpdateDeliveryStatus(int idDelivery, DTO.Status newStatus);
    }
}
