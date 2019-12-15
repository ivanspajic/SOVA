using AutoMapper;
using Data_Layer_Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using SOVA.Models;
using System;
using System.Collections.Generic;

namespace SOVA.Controllers
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

        [Authorize]
        [HttpGet("{submissionId}", Name = nameof(GetAnnotation))]
        public ActionResult GetAnnotation(int submissionId)
        {
            int.TryParse(HttpContext.User.Identity.Name, out var userId);
            var ant = _annotationRepository.GetBySubmissionAndUserIds(submissionId, userId);
            if (ant == null)
            {
                return NoContent();
            }
            return Ok(CreateAnnotationDto(ant));
        }

        [Authorize]
        [HttpGet(Name = nameof(GetAllAnnotationsForUser))]
        public ActionResult GetAllAnnotationsForUser([FromQuery] PagingAttributes pagingAttributes)
        {
            int.TryParse(HttpContext.User.Identity.Name, out var userId);
            var ant = _annotationRepository.GetUserAnnotations(userId, pagingAttributes);
            if (ant == null)
            {
                return NoContent();
            }
            return Ok(CreateResult(ant, userId, pagingAttributes));
        }

        [Authorize]
        [HttpPost("{submissionId}")]
        public ActionResult CreateAnnotation(AnnotationForCreation antDto, int submissionId)
        {
            int.TryParse(HttpContext.User.Identity.Name, out var userId);
            var ant = _mapper.Map<Annotation>(antDto);
            ant.SubmissionId = submissionId;
            ant.UserId = userId;
            _annotationRepository.Create(ant.AnnotationString, submissionId, userId);
            return CreatedAtRoute(
                nameof(GetAnnotation),
                new { submissionId = submissionId, userId = userId },
                CreateAnnotationDto(ant));
        }

        [Authorize]
        [HttpPut("{submissionId}")]
        public ActionResult UpdateAnnotation(int submissionId, Annotation annotation)
        {
            int.TryParse(HttpContext.User.Identity.Name, out var userId);
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

        [Authorize]
        [HttpDelete("{submissionId}")]
        public ActionResult DeleteAnnotation(int submissionId)
        {
            int.TryParse(HttpContext.User.Identity.Name, out var userId);
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

        private object CreateResult(List<Annotation> ant, int userId, PagingAttributes attr)
        {
            var totalItems = _annotationRepository.NoOfAnnotations(userId);
            var numberOfPages = Math.Ceiling((double)totalItems / attr.PageSize);

            var prev = attr.Page > 0
                ? CreatePagingLink(attr.Page - 1, attr.PageSize, nameof(GetAllAnnotationsForUser))
                : null;
            var next = attr.Page < numberOfPages - 1
                ? CreatePagingLink(attr.Page + 1, attr.PageSize, nameof(GetAllAnnotationsForUser))
                : null;
            return new
            {
                totalItems,
                numberOfPages,
                prev,
                next,
                items = ant
            };
        }

        private string CreatePagingLink(int page, int pageSize, string str)
        {
            return Url.Link(str, new { page, pageSize });
        }
    }
}
