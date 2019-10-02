using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.Gateway.HttpAggregator
{
    public interface IUserService
    {
        Task<bool> DetractAmount(int userId, decimal amountToDetract);        
    }
}
