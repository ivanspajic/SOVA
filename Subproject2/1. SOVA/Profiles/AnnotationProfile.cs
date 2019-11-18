using _0._Models;
using _1._SOVA.Models;
using AutoMapper;

namespace _1._SOVA.Profiles
{
    public class AnnotationProfile : Profile
    {
        public AnnotationProfile()
        {
            CreateMap<Annotation, AnnotationDto>();
            CreateMap<AnnotationForCreation, Annotation>();
        }
    }
}
