using System.Collections.Generic;

namespace Reti.Lab.FoodOnKontainers.Basket.Api.Basket.Repository
{
    public interface IBasketRepository
    {
        IEnumerable<string> GetUsers();
    }
}
