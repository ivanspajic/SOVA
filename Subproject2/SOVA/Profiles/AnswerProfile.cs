using AutoMapper;
using Models;
using SOVA.Models;

namespace SOVA.Profiles
{
    public class AnswerProfile : Profile
    {
        public AnswerProfile()
        {
            CreateMap<Answer, AnswerDto>();
        }
    }
}
