using _0._Models;
using _1._SOVA.Models;
using AutoMapper;

namespace _1._SOVA.Profiles
{
    public class QuestionProfile : Profile
    {
        public QuestionProfile()
        {
            CreateMap<Question, QuestionDto>();
        }
    }
}
