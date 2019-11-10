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
        private IMapper _mapper;

        public CommentsController(ICommentRepository commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        [HttpGet(Name = nameof(GetAllCommentsBySubmissionId))]
        public ActionResult GetAllCommentsBySubmissionId(int submissionId)
        {
            var comments = _commentRepository.GetAllCommentsBySubmissionId(submissionId);
            return Ok(CreateResult(comments));
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
    }
}
