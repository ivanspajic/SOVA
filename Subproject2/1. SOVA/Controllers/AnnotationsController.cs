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
            _annotationRepository = annotationRepository;
            _mapper = mapper;
        }

        [HttpGet("{submissionId}", Name = nameof(GetAnnotation))]
        public ActionResult GetAnnotation(int submissionId)
        {
            var ant = _annotationRepository.GetBySubmissionId(submissionId);
            if (ant == null)
            {
                return NotFound();
            }
            return Ok(CreateAnnotationDto(ant));
        }

        [HttpPost("{submissionId}")]
        public ActionResult CreateAnnotation(AnnotationForCreation antDto, int submissionId)
        {
            var ant = _mapper.Map<Annotation>(antDto);
            ant.SubmissionId = submissionId;
            _annotationRepository.Create(ant.AnnotationString, submissionId);
            return CreatedAtRoute(
                nameof(GetAnnotation),
                new { submissionId = ant.SubmissionId },
                CreateAnnotationDto(ant));
        }

        [HttpPut("{submissionId}")]
        public ActionResult UpdateAnnotation(
            int submissionId, Annotation annotation)
        {
            if (_annotationRepository.GetBySubmissionId(submissionId) == null)
            {
                return NotFound();
            }
            annotation.SubmissionId = submissionId;
            _annotationRepository.Update(annotation.AnnotationString, annotation.SubmissionId);
            return Ok();
        }

        [HttpDelete("{submissionId}")]
        public ActionResult DeleteAnnotation(int submissionId)
        {
            if (_annotationRepository.Delete(submissionId))
            {
                return Ok();
            }
            return NotFound();
        }

        ///////////////////
        //
        // Helpers
        //
        ///////////////////

        private AnnotationDto CreateAnnotationDto(Annotation annotation)
        {
            var dto = _mapper.Map<AnnotationDto>(annotation);
            dto.Link = Url.Link(
                    nameof(GetAnnotation),
                    new { annotationId = annotation.SubmissionId });
            return dto;
        }
    }
}
