namespace Reti.Lab.FoodOnKontainers.Users.Api.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public bool Enabled { get; set; }
        public decimal Budget { get; set; }
        public int? RestaurantId { get; set; }
    }
}
