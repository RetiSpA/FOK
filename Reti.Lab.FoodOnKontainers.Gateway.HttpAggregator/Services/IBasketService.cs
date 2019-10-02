using Reti.Lab.FoodOnKontainers.Gateway.HttpAggregator.Dto;
using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.Gateway.HttpAggregator
{
    public interface IBasketService
    {
        Task<BasketData> GetByIdAsync(int basketId);
        Task<string> DeleteBasketAsync(int basketId);
    }
}
