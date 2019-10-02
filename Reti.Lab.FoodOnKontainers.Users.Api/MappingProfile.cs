using AutoMapper;

namespace Reti.Lab.FoodOnKontainers.Users.Api
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Models.User, Dto.User>().ReverseMap();
        }        
    }
}