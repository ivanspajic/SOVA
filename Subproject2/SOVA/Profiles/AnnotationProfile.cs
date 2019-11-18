using AutoMapper;
using Models;
using SOVA.Models;

namespace SOVA.Profiles
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
