using AutoMapper;
using Models;
using SOVA.Models;

namespace SOVA.Profiles
{
    public class UserHistoryProfile : Profile
    {
        public UserHistoryProfile()
        {
            CreateMap<UserHistory, UserHistoryDto>();
        }
    }
}
