using _0._Models;
using _1._SOVA.Models;
using _2._Data_Layer_Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1._SOVA.Controllers
{
    [ApiController]
    [Route("api/Annotations")]
    public class AnnotationsController : ControllerBase
    {
        private readonly IAnnotationRepository _annotationRepository;
        private IMapper _mapper;

        public AnnotationsController(IAnnotationRepository annotationRepository, IMapper mapper)
        {
            this._annotationRepository = annotationRepository;
            this._mapper = mapper;
        }
    }
}
