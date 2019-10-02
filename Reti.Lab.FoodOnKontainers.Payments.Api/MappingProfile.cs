using AutoMapper;

namespace Reti.Lab.FoodOnKontainers.Payments.Api
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Models.Transaction, Dto.Transaction>().ReverseMap();
        }        
    }
}