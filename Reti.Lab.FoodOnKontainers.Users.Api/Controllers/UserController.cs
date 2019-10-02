using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Reti.Lab.FoodOnKontainers.Middleware;
using Reti.Lab.FoodOnKontainers.Users.Api.Dto;

namespace Reti.Lab.FoodOnKontainers.Users.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly User.IUserService _userService;
        private readonly ILogService _logger;

        public UserController(ILogService logger, User.IUserService userSvc)
        {
            _logger = logger;
            _userService = userSvc;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] Login request)
        {            
            var user = _userService.Authenticate(request.Username, request.Password);

            if (user == null)
            {
                _logger.Log("Username or password is incorrect", LogLevel.Error, System.Net.HttpStatusCode.BadRequest, "UserService");
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            _logger.Log("Auth avvenuta con successo", LogLevel.Information, System.Net.HttpStatusCode.OK, "UserService");

            return Ok(user);
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody]Dto.User userParam)
        {
            var result = _userService.Register(userParam);

            if (result == false)
                return BadRequest();

            return Ok();
        }

        [HttpGet("getUser/{userId}")]
        public async Task<IActionResult> GetUser(int userId)
        {
            var user = await _userService.GetUser(userId);
            return Ok(user);
        }

        [HttpPut("updateUser/{id}")]
        public IActionResult UpdateUser([FromBody]Dto.User userParam)
        {
            var result = _userService.UpdateUser(userParam);

            if (result == false)
                return BadRequest();

            return Ok();
        }

        [HttpGet("getAll")]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            _logger.Log("Users recuperati con successo", LogLevel.Information, System.Net.HttpStatusCode.OK, "UserService");
            return Ok(users);
        }

        [HttpGet("addFavorite/{userId}/{restaurantId}")]
        public IActionResult AddFavorite(int userId, int restaurantId)
        {
            var result = _userService.AddFavorite(userId, restaurantId);

            if (result == false)
                return BadRequest();

            return Ok();
        }

        [HttpGet("removeFavorite/{userId}/{restaurantId}")]
        public IActionResult RemoveFavorite(int userId, int restaurantId)
        {
            var result = _userService.RemoveFavorite(userId, restaurantId);

            if (result == false)
                return BadRequest();

            return Ok();
        }

        [HttpGet("detractBudget/{userId}/{amountToDetract}")]
        public IActionResult DetractBudget(int userId, decimal amountToDetract)
        {
            var result = _userService.DetractBudget(userId, amountToDetract);

            if (result == false)
                return BadRequest("The budget is not enough!");

            return Ok();
        }
    }  
}