using AutoMapper;
using Models;
using SOVA.Models;

namespace SOVA.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserForCreation, User>();
        }
    }
}
