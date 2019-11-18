using AutoMapper;
using Models;
using SOVA.Models;

namespace SOVA.Profiles
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<Comment, CommentDto>();
        }
    }
}
