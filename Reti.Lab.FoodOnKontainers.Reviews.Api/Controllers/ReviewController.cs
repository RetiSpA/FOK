using Microsoft.AspNetCore.Mvc;
using Reti.Lab.FoodOnKontainers.Reviews.Api.DTO;
using Reti.Lab.FoodOnKontainers.Reviews.Api.Services;
using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.Reviews.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService reviewService;

        public ReviewController(IReviewService reviewService)
        {
            this.reviewService = reviewService;
        }
        
        [HttpGet("restaurant/{idRestaurant}")]
        public async Task<IActionResult> GetRestaurantReviews([FromRoute] int idRestaurant)
        {
            var restaurantReviews = await this.reviewService.GetRestaurantReviews(idRestaurant);
            return Ok(restaurantReviews);
        }

        [HttpGet("rider/{idRider}")]
        public async Task<IActionResult> GetRiderReviews([FromRoute] int idRider)
        {
            var riderReviews = await this.reviewService.GetRiderReviews(idRider);
            return Ok(riderReviews);
        }

        [HttpGet("restaurant/order/{idOrder}")]
        public async Task<IActionResult> GetRestaurantReviewByOrder([FromRoute] int idOrder)
        {
            var restaurantReview = await this.reviewService.GetRestaurantReviewByOrder(idOrder);
            return Ok(restaurantReview);
        }

        [HttpGet("rider/order/{idOrder}")]
        public async Task<IActionResult> GetRiderReviewByOrder([FromRoute] int idOrder)
        {
            var riderReview = await this.reviewService.GetRiderReviewByOrder(idOrder);
            return Ok(riderReview);
        }

        [HttpGet("restaurant/user/{idUser}")]
        public async Task<IActionResult> GetRestaurantsReviewsByUser([FromRoute] int idUser)
        {
            var restaurantReview = await this.reviewService.GetRestaurantsReviewByUser(idUser);
            return Ok(restaurantReview);
        }

        [HttpGet("rider/user/{idUser}")]
        public async Task<IActionResult> GetRidersReviewsByUser([FromRoute] int idUser)
        {
            var riderReview = await this.reviewService.GetRidersReviewByUser(idUser);
            return Ok(riderReview);
        }

        [HttpGet("rider/{idRider}/{idUser}")]
        public async Task<IActionResult> GetRiderReviewByUser([FromRoute] int idRider, [FromRoute] int idUser )
        {
            var riderReview = await this.reviewService.GetRiderReviewByUser(idUser,idRider);
            return Ok(riderReview);
        }

        [HttpGet("restaurant/{idRestaurant}/{idUser}")]
        public async Task<IActionResult> GetRestaurantReviewByUser([FromRoute] int idRestaurant, [FromRoute] int idUser)
        {
            var restaurantReview = await this.reviewService.GetRestaurantReviewByUser(idUser, idRestaurant);
            return Ok(restaurantReview);
        }

        [HttpPost("restaurant/add")]
        public async Task<IActionResult> AddRestaurantReview([FromBody] RestaurantReviewDto restaurantReviewDto)
        {
            await this.reviewService.AddRestaurantReview(restaurantReviewDto);
            return Ok();
        }

        [HttpPost("rider/add")]
        public async Task<IActionResult> AddRiderReview([FromBody] RiderReviewDto riderReviewDto)
        {
            await this.reviewService.AddRiderReview(riderReviewDto);
            return Ok();
        }

    }
}