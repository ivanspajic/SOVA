using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using _0._Models;
using _1._SOVA.Models;
using _2._Data_Layer_Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace _1._SOVA.Controllers
{
    [ApiController]
    [Route("api/questions")]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionRepository _questionRepository;
        private IMapper _mapper;

        public QuestionsController(IQuestionRepository questionRepository, IMapper mapper)
        {
            _questionRepository = questionRepository;
            _mapper = mapper;
        }

        [HttpGet(Name = nameof(GetQuestions))]
        public ActionResult GetQuestions()
        {
            var questions = _questionRepository.GetTenRandomQuestions();
            return Ok(CreateResult(questions));
        }

        [HttpGet("{questionId}", Name = nameof(GetQuestion))]
        public ActionResult GetQuestion(int questionId)
        {
            var question = _questionRepository.GetById(questionId);
            if (question == null)
            {
                return NotFound();
            }
            return Ok(CreateQuestionDto(question));
        }

        [HttpGet("query/{queryString}", Name = nameof(SearchQuestion))]
        public ActionResult SearchQuestion([FromQuery] PagingAttributes pagingAttributes, string queryString)
        {
            var searchResults = _questionRepository.SearchQuestions(queryString, pagingAttributes);
            return Ok(CreateResult(searchResults, queryString, pagingAttributes));
        }

        ///////////////////
        //
        // Helpers
        //
        ///////////////////

        private QuestionDto CreateQuestionDto(Question question)
        {
            var dto = _mapper.Map<QuestionDto>(question);
            dto.Link = Url.Link(
                    nameof(GetQuestion),
                    new { questionId = question.SubmissionId });
            return dto;
        }

        private IEnumerable<QuestionDto> CreateResult(IEnumerable<Question> questions)
        {
            return questions.Select(q => CreateQuestionDto(q));
        }

        private object CreateResult(IEnumerable<SearchResult> questions, string str, PagingAttributes attr)
        {
            var totalItems = _questionRepository.NoOfResults(str);
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
                items = questions
            };
        }

        private string CreatePagingLink(int page, int pageSize)
        {
            return Url.Link(nameof(SearchQuestion), new { page, pageSize });
        }
    }
}

