using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Data_Layer_Abstractions;
using Microsoft.AspNetCore.Mvc;
using Models;
using SOVA.Models;

namespace SOVA.Controllers
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
            return Ok(CreateResult(comments, submissionId));
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

        private object CreateResult(IEnumerable<Comment> comments, int submissionId)
        {
            return new
            {
                items = comments.Select(CreateCommentDto)
            };
        }
    }
}
