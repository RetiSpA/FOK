using System.ComponentModel.DataAnnotations;

namespace Reti.Lab.FoodOnKontainers.DTO.Common
{
    public class Position
    {
        [Required]
        public double Longitude { get; set; }
        [Required]
        public double Latitude { get; set; }
    }
}
