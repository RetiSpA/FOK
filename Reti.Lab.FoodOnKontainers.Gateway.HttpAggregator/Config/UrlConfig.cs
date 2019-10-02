using System.Globalization;

namespace Reti.Lab.FoodOnKontainers.Gateway.HttpAggregator
{
    public class UrlsConfig
    {
        public class PaymentOperations
        {
            public static string CreateTransaction() => "/api/payment/addTransaction";
        }

        public class BasketOperations
        {
            public static string GetItemById(int userId) => $"/api/basket/user/{userId}";
            public static string DeleteBasket(int userId) => $"/api/basket/user/clear/{userId}";
        }

        public class OrdersOperations
        {
            public static string CreateOrder() => "/api/orders/add";
        }

        public class UserOperations
        {
            public static string DetractAmount(int userId, decimal amountToDetract) => $"/api/user/detractBudget/{userId}/{amountToDetract.ToString(CultureInfo.CreateSpecificCulture("en-US"))}";
        }

        public class RestaurantsOperations
        {
            public static string GetRestaurantById(int restaurantId) => $"/api/restaurants/detail/{restaurantId}";
        }

        public string Basket { get; set; }
        public string Payment { get; set; }
        public string Order { get; set; }
        public string User { get; set; }
        public string Restaurant { get; set; }
    }
}
