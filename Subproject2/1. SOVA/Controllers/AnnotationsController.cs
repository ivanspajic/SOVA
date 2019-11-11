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
    [Route("api/annotation")]
    public class AnnotationsController : ControllerBase
    {
        private readonly IAnnotationRepository _annotationRepository;
        private IMapper _mapper;

        public AnnotationsController(IAnnotationRepository annotationRepository, IMapper mapper)
        {
            _annotationRepository = annotationRepository;
            _mapper = mapper;
        }

        [HttpGet("{submissionId}/{userId}", Name = nameof(GetAnnotation))]
        public ActionResult GetAnnotation(int submissionId, int userId)
        {
            var ant = _annotationRepository.GetBySubmissionAndUserIds(submissionId, userId);
            if (ant == null)
            {
                return NotFound();
            }
            return Ok(CreateAnnotationDto(ant));
        }

        [HttpPost("{submissionId}/{userId}")]
        public ActionResult CreateAnnotation(AnnotationForCreation antDto, int submissionId, int userId)
        {
            var ant = _mapper.Map<Annotation>(antDto);
            ant.SubmissionId = submissionId;
            _annotationRepository.Create(ant.AnnotationString, submissionId, userId);
            return CreatedAtRoute(
                nameof(GetAnnotation),
                new { submissionId = ant.SubmissionId, userId = ant.UserId },
                CreateAnnotationDto(ant));
        }

        [HttpPut("{submissionId}/{userId}")]
        public ActionResult UpdateAnnotation(int submissionId, int userId, Annotation annotation)
        {
            var ant = _annotationRepository.GetBySubmissionAndUserIds(submissionId, userId);
            if (ant == null)
            {
                return NotFound();
            }
            annotation.SubmissionId = submissionId;
            _annotationRepository.Update(annotation.AnnotationString, annotation.SubmissionId, annotation.UserId);
            return Ok();
        }

        [HttpDelete("{submissionId}/{userId}")]
        public ActionResult DeleteAnnotation(int submissionId, int userId)
        {
            if (_annotationRepository.Delete(submissionId, userId))
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
                    new { submissionId = annotation.SubmissionId, userId = annotation.UserId });
            return dto;
        }
    }
}
