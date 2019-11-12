using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _0._Models;
using _1._SOVA.Models;
using _2._Data_Layer_Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace _1._SOVA.Controllers
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

        [HttpGet("{userId}/markedposts", Name = nameof(GetMarkedPostsForUser))]
        public ActionResult GetMarkedPostsForUser([FromQuery] PagingAttributes pagingAttributes, int userId)
        {
            var posts = _markingRepository.GetMarkedPosts(userId, pagingAttributes);
            return Ok(CreateResult(posts, userId, pagingAttributes));
        }

        [HttpPut("{submissionId}/{userId}")]
        public ActionResult UpdateBookmark(int submissionId, int userId)
        {
            if (_markingRepository.IsMarked(submissionId, userId))
            {
                _markingRepository.RemoveBookmark(submissionId, userId);
                return Ok();
            }

            _markingRepository.Bookmark(submissionId, userId);
            return Ok();
        }

        ///////////////////
        //
        // Helpers
        //
        ///////////////////

        private object CreateResult(IEnumerable<Submission> posts, int userId, PagingAttributes attr)
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
                items = posts.Select(CreateSubmissionDto)
            };
        }

        private SubmissionDto CreateSubmissionDto(Submission sub)
        {
            var dto = _mapper.Map<SubmissionDto>(sub);
            dto.Link = Url.Link(
                    nameof(GetMarkedPostsForUser),
                    new { submissionId = sub.Id });
            return dto;
        }

        private string CreatePagingLink(int page, int pageSize)
        {
            return Url.Link(nameof(GetMarkedPostsForUser), new { page, pageSize });
        }
    }
}
