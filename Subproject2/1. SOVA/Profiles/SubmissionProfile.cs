using _0._Models;
using _1._SOVA.Models;
using AutoMapper;

namespace _1._SOVA.Profiles
{
    public class SubmissionProfile : Profile
    {
        public SubmissionProfile()
        {
            CreateMap<Submission, SubmissionDto>();
        }
    }
}
