using APILearning.DTOs;
using APILearning.Entities;
using AutoMapper;

namespace APILearning.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            
            CreateMap<RegisterDto, AppUser>().ReverseMap();
            CreateMap<AppUser, UserDto>().ReverseMap();
        }
    }
}
