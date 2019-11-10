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
    [Route("api/{questionId}/answers")]
    public class AnswersController : ControllerBase
    {
        private readonly IAnswerRepository _answerRepository;
        private IMapper _mapper;

        public AnswersController(IAnswerRepository answerRepository, IMapper mapper)
        {
            _answerRepository = answerRepository;
            _mapper = mapper;
        }

        [HttpGet(Name = nameof(GetAnswersForQuestion))]
        public ActionResult GetAnswersForQuestion([FromQuery] PagingAttributes pagingAttributes, int questionId)
        {
            var answers = _answerRepository.GetAnswersForQuestionById(questionId, pagingAttributes);
            return Ok(CreateResult(answers, questionId, pagingAttributes));
        }


        ///////////////////
        //
        // Helpers
        //
        ///////////////////

        private AnswerDto CreateAnswerDto(Answer answer)
        {
            var dto = _mapper.Map<AnswerDto>(answer);
            dto.Link = Url.Link(
                    nameof(GetAnswersForQuestion),
                    new { answerId = answer.SubmissionId });
            return dto;
        }

        private IEnumerable<AnswerDto> CreateResult(IEnumerable<Answer> answers)
        {
            return answers.Select(a => CreateAnswerDto(a));
        }

        private object CreateResult(IEnumerable<Answer> answers, int questionId, PagingAttributes attr)
        {
            var totalItems = _answerRepository.NoOfAnswers(questionId);
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
                items = answers.Select(CreateAnswerDto)
            };
        }

        private string CreatePagingLink(int page, int pageSize)
        {
            return Url.Link(nameof(GetAnswersForQuestion), new { page, pageSize });
        }
    }
}