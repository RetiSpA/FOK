using Reti.Lab.FoodOnKontainers.Basket.Api.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.Basket.Api.Basket
{
    public interface IBasketService
    {
        Task SetBasket(UserBasket userBasket);
        Task ClearBasket(int userId);
        Task AddItem(UserBasket userBasket);
        Task UpdateItem(int userId, int itemId, int newQuantity);
        Task UpdateItem(int userId, int itemId, decimal newPrice);
        Task UpdateItem(int userId, int itemId, bool available);
        Task RemoveItem(int userId, int itemId);
        Task<UserBasket> GetBasket(int userId);
        IEnumerable<string> GetItems();
    }
}
