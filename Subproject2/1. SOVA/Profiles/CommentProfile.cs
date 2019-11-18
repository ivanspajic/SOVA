using _0._Models;
using _1._SOVA.Models;
using AutoMapper;

namespace _1._SOVA.Profiles
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<Comment, CommentDto>();
        }
    }
}
