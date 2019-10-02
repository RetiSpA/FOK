using System;
using System.Collections.Generic;

namespace Reti.Lab.FoodOnKontainers.Payments.Api.Models
{
    public partial class Receipt
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public bool Delivered { get; set; }
    }
}
