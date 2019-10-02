using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Reti.Lab.FoodOnKontainers.DTO.Orders;
using Reti.Lab.FoodOnKontainers.Payments.Api.Dal;
using Reti.Lab.FoodOnKontainers.Payments.Api.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Transaction = Reti.Lab.FoodOnKontainers.Payments.Api.Dto.Transaction;

namespace Reti.Lab.FoodOnKontainers.Payments.Api.Payment
{
    public class PaymentService : IPaymentService
    {
        readonly IConfiguration _config;
        private readonly Dto.AppSettings _appSettings;
        private readonly PaymentDbContext _paymentDbContext;
        private readonly IMapper _mapper;

        public PaymentService(IConfiguration config, IOptions<Dto.AppSettings> appSettings, IMapper mapper, PaymentDbContext dbContext)
        {
            _config = config;
            _appSettings = appSettings.Value;
            _mapper = mapper;
            _paymentDbContext = dbContext;
        }     

        public async Task<bool> AddTransaction(Transaction transaction)
        {
            var transactionDb = _mapper.Map<Models.Transaction>(transaction);
            transactionDb.Date = DateTime.Now;

            var result = _paymentDbContext.Transaction.Add(transactionDb);

            if (result.State != Microsoft.EntityFrameworkCore.EntityState.Added)
                return false;

            await _paymentDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<string> GetReceiptAsync(int orderId)
        {
            Models.Transaction transactionDb = _paymentDbContext.Transaction
                .FirstOrDefault(x => x.OrderId == orderId && x.Status.Equals(TransactionStatusEnum.Closed.ToString()));

            if (transactionDb == null)
                return null;

            Models.Receipt receiptDb =_paymentDbContext.Receipt
                .FirstOrDefault(x => x.TransactionId == transactionDb.Id && x.Delivered == false);

            if (receiptDb == null)
                return null;            

            HttpClient client = new HttpClient();
            string apiPath = string.Format("{0}{1}", _appSettings.OrdersAPI, transactionDb.OrderId);

            HttpResponseMessage response = await client.GetAsync(apiPath);

            Order order = null;

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                order = JsonConvert.DeserializeObject<Order>(json.ToString());
            }   

            Receipt receipt = new Receipt()
            {
                Date = DateTime.Now,
                OrderItem = order.OrderItem,
                RestaurantAddress = order.RestaurantAddress,
                RestaurantName = order.RestaurantName,
                TotalPrice = order.Price
            };

            receiptDb.Delivered = true;
            _paymentDbContext.SaveChanges();
            return JsonConvert.SerializeObject(receipt);
        }

        public async Task<bool> UpdateTransaction(Transaction transaction)
        {
            var transactionDb = _mapper.Map<Models.Transaction>(transaction);
            transactionDb.Date = DateTime.Now;

            var result = _paymentDbContext.Transaction.Update(transactionDb);

            if (result.State != Microsoft.EntityFrameworkCore.EntityState.Modified)
                return false;

            await _paymentDbContext.SaveChangesAsync();
            return true;
        }

        async Task IPaymentService.CheckReceipt()
        {
            IEnumerable<Models.Transaction> confirmedTransactions = _paymentDbContext.Transaction
                .Where(x => x.Status.Equals(TransactionStatusEnum.Confirmed.ToString()));

            foreach (var confirmedTransaction in confirmedTransactions)
            {
                int transactionId = confirmedTransaction.Id;
                var result = _paymentDbContext.Receipt.Add(new Models.Receipt() { TransactionId = confirmedTransaction.Id, Delivered = false });

                if (result.State == Microsoft.EntityFrameworkCore.EntityState.Added)
                    _paymentDbContext.Transaction.FirstOrDefault(x => x.Id.Equals(confirmedTransaction.Id)).Status = TransactionStatusEnum.Closed.ToString();
            }

            await _paymentDbContext.SaveChangesAsync();            
        }
    }
}
