using Microsoft.AspNetCore.Mvc;
using Reti.Lab.FoodOnKontainers.Middleware;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.Payments.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {

        private readonly Payment.IPaymentService _paymentService;
        private readonly ILogService _logger;

        public PaymentController(ILogService logger, Payment.IPaymentService paymentSvc)
        {
            _logger = logger;
            _paymentService = paymentSvc;
        }

        // GET api/payment
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "Hello from PAYMENTS microservice!!!" };
        }

        //GET api/payment
        [HttpGet("getReceipt/{orderId}")]
        public async Task<IActionResult> GetReceipt(int orderId)
        {
            var result = await _paymentService.GetReceiptAsync(orderId);
            return Ok(result);
        }

        // PUT api/payment
        [HttpPut("updateTransaction")]
        public async Task<IActionResult> UpdateTransaction([FromBody]Dto.Transaction transactionParam)
        {
            var result = await _paymentService.UpdateTransaction(transactionParam);

            if (result == false)
                return BadRequest();

            return Ok();
        }

        // POST api/payment
        [HttpPost("addTransaction")]
        public async Task<IActionResult> AddTransaction([FromBody]Dto.Transaction transactionParam)
        {
            var result = await _paymentService.AddTransaction(transactionParam);

            if (result == false)
                return BadRequest();

            return Ok();
        }
    }
}