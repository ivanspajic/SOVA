using _0._Models;
using _1._SOVA.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
