using AutoMapper;
using Models;
using SOVA.Models;

namespace SOVA.Profiles
{
    public class QuestionProfile : Profile
    {
        public QuestionProfile()
        {
            CreateMap<Question, QuestionDto>();
        }
    }
}
