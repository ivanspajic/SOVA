﻿using _2._Data_Layer_Abstractions;
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
        private readonly IAnnotationRepository annotationRepository;
        private IMapper mapper;

        public AnnotationsController(IAnnotationRepository annotationRepository, IMapper mapper)
        {
            this.annotationRepository = annotationRepository;
            this.mapper = mapper;
        }


    }
}