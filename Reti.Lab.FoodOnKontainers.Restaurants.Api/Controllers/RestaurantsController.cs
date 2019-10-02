using Microsoft.AspNetCore.Mvc;
using Reti.Lab.FoodOnKontainers.Restaurants.Api.DTO;
using Reti.Lab.FoodOnKontainers.Restaurants.Api.Mappers;
using Reti.Lab.FoodOnKontainers.Restaurants.Api.Services;
using System.Threading.Tasks;


namespace Reti.Lab.FoodOnKontainers.Restaurants.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly IRestaurantService restaurantService;

        public RestaurantsController(IRestaurantService restaurantService)
        {
            this.restaurantService = restaurantService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetRestaurants()
        {
            var restaurants = await restaurantService.GetRestaurants();
            return Ok(restaurants);
        }

        [HttpGet("detail/{idRestaurant}")]
        public async Task<IActionResult> GetRestaurantDetail([FromRoute]int idRestaurant)
        {
            var restaurantDetail = await restaurantService.GetRestaurantDetail(idRestaurant);
            return Ok(restaurantDetail);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddRestaurant([FromBody]RestaurantDTO restaurant)
        {
            await restaurantService.AddRestaurant(RestaurantsMapper.MapNewRestaurantDTO(restaurant));
            return Ok();
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateRestaurant([FromBody]RestaurantDTO restaurant)
        {
            await restaurantService.UpdateRestaurant(RestaurantsMapper.MapRestaurantDTO(restaurant));
            return Ok();
        }

        [HttpPut("update/rating")]
        public async Task<IActionResult> UpdateRestaurantRating([FromBody]RestaurantRatingDTO restaurantRating)
        {
            await restaurantService.UpdateRestaurantAverageRating(restaurantRating.idRestaurant, restaurantRating.averageRating);
            return Ok();
        }

        [HttpDelete("delete/{idRestaurant}")]
        public async Task<IActionResult> DeleteRestaurant([FromRoute]int idRestaurant)
        {
            await restaurantService.DisableRestaurant(idRestaurant);
            return Ok();
        }

    }
}