﻿using System;

namespace Reti.Lab.FoodOnKontainers.Payments.Api.Dto
{
    public class Transaction
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int OrderId { get; set; }
        public decimal Total { get; set; }
        public DateTime Date{ get; set; }
        public int RestaurantId { get; set; }
        public int PaySystem { get; set; }
        public string Status { get; set; }
    }
}