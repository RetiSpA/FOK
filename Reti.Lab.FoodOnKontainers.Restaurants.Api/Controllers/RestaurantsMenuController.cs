using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Reti.Lab.FoodOnKontainers.Restaurants.Api.DTO;
using Reti.Lab.FoodOnKontainers.Restaurants.Api.Models;
using Reti.Lab.FoodOnKontainers.Restaurants.Api.Services;

namespace Reti.Lab.FoodOnKontainers.Restaurants.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsMenuController : ControllerBase
    {
        private readonly IRestaurantMenuService restaurantMenuService;

        public RestaurantsMenuController(IRestaurantMenuService restaurantMenuService)
        {
            this.restaurantMenuService = restaurantMenuService;
        }

        [HttpGet("restaurant/{idRestaurant}")]
        public async Task<IActionResult> GetRestaurantMenu(int idRestaurant)
        {
            var menu = await restaurantMenuService.GetRestaurantMenu(idRestaurant);
            return Ok(menu);
        }

        [HttpPost("new")]
        public async Task<IActionResult> AddMenuItem([FromBody] MenuItemDTO newMenuItemDto)
        {
            //mapping DTO to Model
            var newMenu = new RestaurantsMenu
            {
                Description = newMenuItemDto.Description,
                IdDishType = newMenuItemDto.IdDishType,
                IdRestaurant = newMenuItemDto.IdRestaurant,
                Name = newMenuItemDto.Name,
                Price = newMenuItemDto.Price,
                Promo = newMenuItemDto.Promo
            };

            await restaurantMenuService.AddMenuItem(newMenu);
            return Ok();
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateMenuItem([FromBody] MenuItemDTO menuItemDto)
        {
            //mapping DTO to Model
            var updatedMenu = new RestaurantsMenu
            {
                Id = menuItemDto.Id,
                Description = menuItemDto.Description,
                IdDishType = menuItemDto.IdDishType,
                IdRestaurant = menuItemDto.IdRestaurant,
                Name = menuItemDto.Name,
                Price = menuItemDto.Price,
                Promo = menuItemDto.Promo
            };

            await restaurantMenuService.UpdateMenuItem(updatedMenu);
            return Ok();
        }

        [HttpDelete("{idMenuItem}")]
        public async Task<IActionResult> DeleteMenuItem([FromRoute] int idMenuItem)
        {
            await restaurantMenuService.DeleteMenuItem(idMenuItem);
            return Ok();
        }
    }
}