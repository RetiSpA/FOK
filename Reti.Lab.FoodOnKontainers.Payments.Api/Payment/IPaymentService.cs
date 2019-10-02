using Reti.Lab.FoodOnKontainers.Payments.Api.Dto;
using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.Payments.Api.Payment
{
    public interface IPaymentService
    {
        Task<bool> AddTransaction(Transaction transaction);
        Task<bool> UpdateTransaction(Transaction transaction);
        Task<string> GetReceiptAsync(int orderId);
        Task CheckReceipt();
    }
}
