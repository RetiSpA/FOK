using Microsoft.AspNetCore.Mvc;
using Reti.Lab.FoodOnKontainers.Deliveries.Api.Services.Interfaces;
using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.Deliveries.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RidersController : ControllerBase
    {
        private readonly IRiderService riderService;

        public RidersController(IRiderService riderService)
        {
            this.riderService = riderService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetRiders([FromQuery] DTO.RiderFilter filter)
        {
            var result = await riderService.GetRiders(filter);
            return Ok(result);
        }

        [HttpGet("{idRider}")]
        public async Task<IActionResult> GetRider([FromRoute] int idRider)
        {
            var result = await riderService.GetRider(idRider);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddRider([FromBody] DTO.Rider rider)
        {
            var result = await riderService.AddRider(rider);
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateRider([FromBody] DTO.Rider rider)
        {
            await riderService.UpdateRider(rider);
            return Ok();
        }

        [HttpDelete("delete/{idRider}")]
        public async Task<IActionResult> DeleteRider([FromRoute] int idRider)
        {
            await riderService.UpdateRider(new DTO.Rider() { Active = false, IdRider = idRider });
            return Ok();
        }
    }
}
