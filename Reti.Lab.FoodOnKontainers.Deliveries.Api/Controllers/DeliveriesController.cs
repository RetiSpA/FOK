using Microsoft.AspNetCore.Mvc;
using Reti.Lab.FoodOnKontainers.Deliveries.Api.Services.Interfaces;
using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.Deliveries.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveriesController : ControllerBase
    {
        private readonly IDeliveryService deliveryService;

        public DeliveriesController(IDeliveryService deliveryService)
        {
            this.deliveryService = deliveryService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetDeliveries([FromQuery] DTO.DeliveryFilter filter)
        {
            var result = await deliveryService.GetDeliveries(filter);
            return Ok(result);
        }

        [HttpGet("{idDelivery}")]
        public async Task<IActionResult> GetDelivery([FromRoute] int idDelivery)
        {
            var result = await deliveryService.GetDelivery(idDelivery);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddDelivery([FromBody] DTO.Delivery delivery)
        {
            var result = await deliveryService.AddDelivery(delivery);
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateDelivery([FromBody] DTO.Delivery delivery)
        {
            await deliveryService.UpdateDelivery(delivery);
            return Ok();
        }

        [HttpPut("update/delivery/{idDelivery}/{idStatus}")]
        public async Task<IActionResult> UpdateDeliveryStatus(
            [FromRoute] int idDelivery,
            [FromRoute] int idStatus
            )
        {
            await deliveryService.UpdateDeliveryStatus(idDelivery, (DTO.Status)idStatus);
            return Ok();
        }
    }
}
