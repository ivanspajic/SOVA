using _0._Models;
using _1._SOVA.Models;
using _2._Data_Layer_Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace _1._SOVA.Controllers
{
    [ApiController]
    [Route("api/annotations")]
    public class AnnotationsController : ControllerBase
    {
        private readonly IAnnotationRepository _annotationRepository;
        private IMapper _mapper;

        public AnnotationsController(IAnnotationRepository annotationRepository, IMapper mapper)
        {
            _annotationRepository = annotationRepository;
            _mapper = mapper;
        }

        //[Authorize]
        [HttpGet("{submissionId}", Name = nameof(GetAnnotation))]
        public ActionResult GetAnnotation(int submissionId)
        {
            var userId = int.TryParse(HttpContext.User.Identity.Name, out var id) ? id : 1;
            var ant = _annotationRepository.GetBySubmissionAndUserIds(submissionId, userId);
            if (ant == null)
            {
                return NotFound();
            }
            return Ok(CreateAnnotationDto(ant));
        }

        //[Authorize]
        [HttpPost("{submissionId}")]
        public ActionResult CreateAnnotation(AnnotationForCreation antDto, int submissionId)
        {
            var userId = int.TryParse(HttpContext.User.Identity.Name, out var id) ? id : 1;
            var ant = _mapper.Map<Annotation>(antDto);
            ant.SubmissionId = submissionId;
            ant.UserId = userId;
            _annotationRepository.Create(ant.AnnotationString, submissionId, userId);
            return CreatedAtRoute(
                nameof(GetAnnotation),
                new { submissionId = submissionId, userId = userId },
                CreateAnnotationDto(ant));
        }

        //[Authorize]
        [HttpPut("{submissionId}")]
        public ActionResult UpdateAnnotation(int submissionId, Annotation annotation)
        {
            var userId = int.TryParse(HttpContext.User.Identity.Name, out var id) ? id : 1;
            var ant = _annotationRepository.GetBySubmissionAndUserIds(submissionId, userId);
            if (ant == null)
            {
                return NotFound();
            }
            annotation.SubmissionId = submissionId;
            annotation.UserId = userId;
            _annotationRepository.Update(annotation.AnnotationString, submissionId, userId);
            return Ok(annotation);
        }

        //[Authorize]
        [HttpDelete("{submissionId}")]
        public ActionResult DeleteAnnotation(int submissionId)
        {
            var userId = int.TryParse(HttpContext.User.Identity.Name, out var id) ? id : 1;
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
