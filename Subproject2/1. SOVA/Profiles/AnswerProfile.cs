using _0._Models;
using _1._SOVA.Models;
using AutoMapper;

namespace _1._SOVA.Profiles
{
    public class AnswerProfile : Profile
    {
        public AnswerProfile()
        {
            CreateMap<Answer, AnswerDto>();
        }
    }
}
