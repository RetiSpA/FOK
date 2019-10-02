using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.Deliveries.Api.Services.Interfaces
{
    public interface IRiderService
    {
        Task<List<Models.Rider>> GetRiders(DTO.RiderFilter filter);
        Task<Models.Rider> GetRider(int idRider);
        Task<int> AddRider(DTO.Rider rider);
        Task UpdateRider(DTO.Rider rider);
    }
}
