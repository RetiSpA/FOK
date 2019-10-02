using Microsoft.AspNetCore.Mvc;
using Reti.Lab.FoodOnKontainers.Gateway.HttpAggregator.Dto;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.Gateway.HttpAggregator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlowController : ControllerBase
    {
        private readonly IPaymentService _payment;
        private readonly IBasketService _basket;
        private readonly IUserService _user;
        private readonly IOrderService _order;
        private readonly IRestaurantService _restaurant;

        public FlowController(
            IPaymentService paymentService,
            IBasketService basketService,
            IUserService userService,
            IOrderService orderService,
            IRestaurantService restaurantService
            )
        {
            _payment = paymentService;
            _basket = basketService;
            _user = userService;
            _order = orderService;
            _restaurant = restaurantService;
        }

        [HttpPost("user/confirmBasket")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> ConfirmBasketAsync(ConfirmBasketDto request)
        {
            if (!request.UserId.HasValue)
            {
                return BadRequest("UserId is mandatory!");
            }

            // Retrieve the current basket
            var basketResponse = await _basket.GetByIdAsync(request.UserId.Value);
            if (basketResponse == null || !basketResponse.BasketItems.Any())
                return BadRequest("Basket doesn't exist!");

            // Detract basket total price from user money availability
            decimal amountToDetract = 0M;
            foreach (var item in basketResponse.BasketItems)
            {
                amountToDetract += item.Price * item.Quantity;
            }
            bool isThereBudget = await _user.DetractAmount(basketResponse.UserId, amountToDetract);
            if (isThereBudget == false)
                return BadRequest("Budget is not enought!");

            // Retrieve restaurant data
            var restaurantResponse = await _restaurant.GetRestaurantAsync(basketResponse.RestaurantId);
            if (restaurantResponse == null)
            {
                return BadRequest("Invalid restaurant!");
            }

            // Create order
            OrderData order = new OrderData()
            {
                IdRestaurant = basketResponse.RestaurantId,
                RestaurantName = basketResponse.RestaurantName,
                IdUser = basketResponse.UserId,
                UserName = request.UserName,
                Price = amountToDetract,
                RestaurantAddress = restaurantResponse.Address,
                RestaurantPosition = restaurantResponse.PositionCoordinates,
                DeliveryAddress = request.DeliveryAddress,
                DeliveryPosition = request.DeliveryPosition,
                DeliveryRequestedDate = request.DeliveryRequestedDate,
                OrderItem = basketResponse
                    .BasketItems
                    .Select(i => new OrderItem() { IdMenuItem = i.MenuItemId, MenuItemName = i.MenuItemName, Price = i.Price, Quantity = i.Quantity })
                    .ToList()
            };

            var orderResponse = _order.CreateOrderAsync(order);
            if (!orderResponse.Result.HasValue)
                return BadRequest("Order failed!");

            // Create payment
            TransactionData transaction = new TransactionData()
            {
                Date = DateTime.UtcNow,
                OrderId = orderResponse.Result.Value,
                PaySystem = 1,
                RestaurantId = basketResponse.RestaurantId,
                Status = TransactionStatus.Pending,
                Total = amountToDetract,
                UserId = basketResponse.UserId
            };

            var paymentResponse = _payment.CreateTransactionAsync(transaction);
            if (paymentResponse.Result == false)
                return BadRequest("Transaction failed!");
            else transaction.Status = TransactionStatus.Confirmed;

            // Delete basket
            await _basket.DeleteBasketAsync(request.UserId.Value);

            return true;
        }
    }
}
