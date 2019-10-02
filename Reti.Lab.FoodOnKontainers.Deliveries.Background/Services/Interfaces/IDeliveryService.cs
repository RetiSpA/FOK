using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.Deliveries.Background.Services.Interfaces
{
    public interface IDeliveryService
    {
        IEnumerable<Models.Delivery> GetDeliveriesToAssign();
        Task<Models.Delivery> GetDelivery(int idDelivery);
        Task UpdateDelivery(DTO.Delivery delivery);
    }
}
