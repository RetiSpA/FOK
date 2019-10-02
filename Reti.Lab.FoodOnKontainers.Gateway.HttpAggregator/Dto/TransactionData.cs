using System;

namespace Reti.Lab.FoodOnKontainers.Gateway.HttpAggregator.Dto
{
    public class TransactionData
    {
        public int UserId { get; set; }
        public int OrderId { get; set; }
        public decimal Total { get; set; }
        public DateTime Date { get; set; }
        public int RestaurantId { get; set; }
        public int PaySystem { get; set; }
        public TransactionStatus Status { get; set; }
    }

    public enum TransactionStatus
    {
        Pending = 1,
        Confirmed = 2
    }
}
