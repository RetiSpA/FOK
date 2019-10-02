using Microsoft.AspNetCore.Mvc;
using Reti.Lab.FoodOnKontainers.Basket.Api.Basket;
using Reti.Lab.FoodOnKontainers.Basket.Api.Dto;
using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.Basket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService basketService;

        public BasketController(IBasketService _basketSvc)
        {
            basketService = _basketSvc;
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUserBasket([FromRoute]int id)
        {
            var userBasket  = await basketService.GetBasket(id);

            if (userBasket != null)
                return Ok(userBasket);

            return NotFound();
        }

        [HttpPost("user/setbasket")]
        public async Task<IActionResult> SetUserBasket([FromBody]UserBasket userBasket)
        {          
            await basketService.SetBasket(userBasket);
            return Ok();
        }

        [HttpDelete("user/clear/{id}")]
        public async Task<IActionResult> ClearUserBasket([FromRoute]int id)
        {
            await basketService.ClearBasket(id);
            return Ok();
        }

        [HttpPut("user/removeitem")]
        public async Task<IActionResult> RemoveItemFromBasket([FromBody]UserBasketItemToChange basketItemToRemove)
        {
            await basketService.RemoveItem(basketItemToRemove.id, basketItemToRemove.itemId);
            return Ok();
        }

        [HttpPut("user/update")]
        public async Task<IActionResult> UpdateUserBasket([FromBody]UserBasketItemToChange basketItemUpdated)
        {
            await basketService.UpdateItem(basketItemUpdated.id, basketItemUpdated.itemId, basketItemUpdated.quantity);
            return Ok();
        }

        [HttpPost("user/additem")]
        public async Task<IActionResult> AddUserBasketItem([FromBody]UserBasket userBasket)
        {
            await basketService.AddItem(userBasket);
            return Ok();
        }

    }
}