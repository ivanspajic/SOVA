using AutoMapper;
using Data_Layer_Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using SOVA.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SOVA.Controllers
{
    [ApiController]
    [Route("api/")]
    public class MarkingsController : ControllerBase
    {
        private readonly IMarkingRepository _markingRepository;
        private IMapper _mapper;

        public MarkingsController(IMarkingRepository markingRepository, IMapper mapper)
        {
            _markingRepository = markingRepository;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet("bookmarks", Name = nameof(GetMarkedPostsForUser))]
        public ActionResult GetMarkedPostsForUser([FromQuery] PagingAttributes pagingAttributes)
        {
            int.TryParse(HttpContext.User.Identity.Name, out var userId);
            var posts = _markingRepository.GetMarkedSubmissions(userId, pagingAttributes);
            if (posts == null)
            {
                return NoContent();
            }
            return Ok(CreateResult(posts, userId, pagingAttributes));
        }

        [Authorize]
        [HttpPut("{submissionId}/bookmarks")]
        public ActionResult UpdateBookmark(int submissionId)
        {
            int.TryParse(HttpContext.User.Identity.Name, out var userId);
            if (_markingRepository.IsMarked(submissionId, userId))
            {
                if (_markingRepository.RemoveBookmark(submissionId, userId))
                {
                    return Ok(new { message = $"Successfully removed bookmark. Submission with id {submissionId} is now removed from your bookmarks." });
                }

                return BadRequest(new { message = "Failed. Error while removing bookmark." });
            }

            _markingRepository.AddBookmark(submissionId, userId);
            return Ok(new { message = $"Successfully bookmarked. Submission with id {submissionId} is now bookmarked." });
        }

        [Authorize]
        [HttpGet("{submissionId}/checkIfBookmarked")]
        public ActionResult checkBookmark(int submissionId)
        {
            int.TryParse(HttpContext.User.Identity.Name, out var userId);
            if (_markingRepository.IsMarked(submissionId, userId))
            {
                return Ok(new { message = "Already bookmarked." });
            }
            else
            {
                return Ok(new { message = "Not bookmarked." });
            }
        }

        ///////////////////
        //
        // Helpers
        //
        ///////////////////

        private object CreateResult(IEnumerable<Question> posts, int userId, PagingAttributes attr)
        {
            var totalItems = _markingRepository.NoOfMarkings(userId);
            var numberOfPages = Math.Ceiling((double)totalItems / attr.PageSize);

            var prev = attr.Page > 0
                ? CreatePagingLink(attr.Page - 1, attr.PageSize)
                : null;
            var next = attr.Page < numberOfPages - 1
                ? CreatePagingLink(attr.Page + 1, attr.PageSize)
                : null;

            return new
            {
                totalItems,
                numberOfPages,
                prev,
                next,
                items = posts.Select(CreateQuestionDto)
            };
        }

        private QuestionDto CreateQuestionDto(Question q)
        {
            var dto = _mapper.Map<QuestionDto>(q);
            dto.Link = Url.Link(
                    nameof(GetMarkedPostsForUser),
                    new { questionId = q.SubmissionId });
            return dto;
        }

        private string CreatePagingLink(int page, int pageSize)
        {
            return Url.Link(nameof(GetMarkedPostsForUser), new { page, pageSize });
        }
    }
}
