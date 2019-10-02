using Reti.Lab.FoodOnKontainers.Gateway.HttpAggregator.Dto;
using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.Gateway.HttpAggregator
{
    public interface IOrderService
    {
        Task<int?> CreateOrderAsync(OrderData order);
    }
}
