using AutoMapper;
using Models;
using SOVA.Models;

namespace SOVA.Profiles
{
    public class SubmissionProfile : Profile
    {
        public SubmissionProfile()
        {
            CreateMap<Submission, SubmissionDto>();
        }
    }
}
