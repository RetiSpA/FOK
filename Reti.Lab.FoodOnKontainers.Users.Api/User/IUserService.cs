using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.Users.Api.User
{
    public interface IUserService
    {
        Dto.User Authenticate(string username, string password);
        IEnumerable<Dto.User> GetAll();
        Task<Dto.User> GetUser(int userId);
        bool UpdateUser(Dto.User user);
        bool Register(Dto.User user);
        bool Login(Dto.User user);
        bool AddFavorite(int userId, int restaurantId);
        bool RemoveFavorite(int userId, int restaurantId);
        bool DetractBudget(int userdId, decimal amountToDetract);
    }
}
