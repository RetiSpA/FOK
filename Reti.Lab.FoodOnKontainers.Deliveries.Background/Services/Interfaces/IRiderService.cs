using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.Deliveries.Background.Services.Interfaces
{
    public interface IRiderService
    {
        Task<List<Models.Rider>> GetRiders(DTO.RiderFilter filter);
    }
}
