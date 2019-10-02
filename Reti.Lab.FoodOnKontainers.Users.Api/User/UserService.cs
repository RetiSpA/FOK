using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Reti.Lab.FoodOnKontainers.Users.Api.Dal;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.Users.Api.User
{
    public class UserService : IUserService
    {
        readonly IConfiguration _config;
        private readonly Dto.AppSettings _appSettings;
        private readonly UserDbContext _userDbContext;
        private readonly IMapper _mapper;

        public UserService(IConfiguration config, IOptions<Dto.AppSettings> appSettings, IMapper mapper, UserDbContext dbContext)
        {
            _config = config;
            _appSettings = appSettings.Value;
            _mapper = mapper;
            _userDbContext = dbContext;
        }

        public Dto.User Authenticate(string username, string password)
        {            
            var userDb = _userDbContext.User.SingleOrDefault(x => x.Username == username && x.Password == password);

            // return null if user not found
            if (userDb == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            string userId = userDb.Id.ToString();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userId)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            userDb.Token = tokenHandler.WriteToken(token);

            // remove password before returning
            userDb.Password = null;

            Dto.User userDto = _mapper.Map<Dto.User>(userDb);
            return userDto;
        }

        IEnumerable<Dto.User> IUserService.GetAll()
        {
            var usersDb = _userDbContext.User;
            // return users without passwords
            foreach (Models.User user in usersDb)
            {
                user.Password = null;
            }

            return _mapper.Map<IEnumerable<Dto.User>>(usersDb);
        }

        public async Task<Dto.User> GetUser(int userId)
        {
            var userDb = await _userDbContext.User.FirstOrDefaultAsync(x => x.Id == userId);

            // return users without passwords
            userDb.Password = null;

            return _mapper.Map<Dto.User>(userDb);
        }

        public bool UpdateUser(Dto.User userDto)
        {
            var userDb = _mapper.Map<Models.User>(userDto);
            var result = _userDbContext.User.Update(userDb);

            if (result.State != Microsoft.EntityFrameworkCore.EntityState.Modified)
                return false;

            _userDbContext.SaveChanges();
            return true;
        }

        public bool Register(Dto.User userDto)
        {            
            Models.User userDb = _mapper.Map<Models.User>(userDto);            
            var result = _userDbContext.User.Add(userDb);

            if (result.State != Microsoft.EntityFrameworkCore.EntityState.Added)
                return false;

            _userDbContext.SaveChanges();
            return true;
        }

        public bool Login(Dto.User user)
        {
            throw new NotImplementedException();
        }

        public bool AddFavorite(int userId, int restaurantId)
        {
            var result = _userDbContext.Favorite.Add(new Models.Favorite() { RestaurantId = restaurantId, UserId = userId });

            if (result.State != Microsoft.EntityFrameworkCore.EntityState.Added)
                return false;

            _userDbContext.SaveChanges();
            return true;
        }

        public bool RemoveFavorite(int userId, int restaurantId)
        {
            var result = _userDbContext.Favorite.Remove(new Models.Favorite() { RestaurantId = restaurantId, UserId = userId });

            if (result.State != Microsoft.EntityFrameworkCore.EntityState.Deleted)
                return false;

            _userDbContext.SaveChanges();
            return true;
        }

        public bool DetractBudget(int userdId, decimal amountToDetract)
        {
            var user = _userDbContext.User.Where(x => x.Id == userdId).FirstOrDefault();

            if (user.Budget < amountToDetract)
                return false;

            user.Budget -= amountToDetract;

            _userDbContext.SaveChanges();
            return true;
        }
    }
}
