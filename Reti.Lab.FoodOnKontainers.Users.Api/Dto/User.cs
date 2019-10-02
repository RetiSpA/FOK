namespace Reti.Lab.FoodOnKontainers.Users.Api.Dto
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public decimal Budget { get; set; }
        public string Role { get; set; }
        public int? RestaurantId { get; set; }
    }
}
