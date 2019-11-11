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
    [Route("api/{submissionId}/comments")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public CommentsController(ICommentRepository commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        [HttpGet(Name = nameof(GetAllCommentsBySubmissionId))]
        public ActionResult GetAllCommentsBySubmissionId([FromQuery] PagingAttributes pagingAttributes, int submissionId)
        {
            var comments = _commentRepository.GetAllCommentsBySubmissionId(submissionId, pagingAttributes);
            return Ok(CreateResult(comments, submissionId, pagingAttributes));
        }

        ///////////////////
        //
        // Helpers
        //
        ///////////////////

        private CommentDto CreateCommentDto(Comment comment)
        {
            var dto = _mapper.Map<CommentDto>(comment);
            dto.Link = Url.Link(
                nameof(GetAllCommentsBySubmissionId),
                new { commentId = comment.Id });
            return dto;
        }

        private IEnumerable<CommentDto> CreateResult(IEnumerable<Comment> comments)
        {
            return comments.Select(c => CreateCommentDto(c));
        }

        private object CreateResult(IEnumerable<Comment> comments, int submissionId, PagingAttributes attr)
        {
            var totalItems = _commentRepository.NoOfComments(submissionId);
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
                items = comments.Select(CreateCommentDto)
            };
        }

        private string CreatePagingLink(int page, int pageSize)
        {
            return Url.Link(nameof(GetAllCommentsBySubmissionId), new { page, pageSize });
        }
    }
}
