using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _0._Models;
using AutoMapper;
using _1._SOVA.Models;

namespace _1._SOVA.Profiles
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
