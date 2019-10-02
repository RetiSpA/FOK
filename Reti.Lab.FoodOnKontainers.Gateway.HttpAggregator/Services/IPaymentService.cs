using Reti.Lab.FoodOnKontainers.Gateway.HttpAggregator.Dto;
using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.Gateway.HttpAggregator
{
    public interface IPaymentService
    {
        Task<bool> CreateTransactionAsync(TransactionData transaction);
    }
}
