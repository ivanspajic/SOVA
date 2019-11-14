using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpGet("{questionId}/answers", Name = nameof(GetAnswersForQuestion))]
        public ActionResult GetAnswersForQuestion([FromQuery] PagingAttributes pagingAttributes, int questionId)
        {
            var answers = _questionRepository.GetAnswersForQuestionById(questionId, pagingAttributes);
            return Ok(CreateResult(answers, questionId, pagingAttributes));
        }

        //[Authorize]
        [HttpGet("query/{queryString}", Name = nameof(SearchQuestion))]
        public ActionResult SearchQuestion([FromQuery] PagingAttributes pagingAttributes, string queryString)
        {
            var userId = int.TryParse(HttpContext.User.Identity.Name, out var id) ? id : 1;
            var searchResults = _questionRepository.SearchQuestions(queryString, userId, pagingAttributes);
            return Ok(CreateResult(searchResults, queryString, userId, pagingAttributes));
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

        private object CreateResult(IEnumerable<SearchResult> questions, string str, int userId, PagingAttributes attr)
        {
            var totalItems = _questionRepository.NoOfResults(str, userId);
            var numberOfPages = Math.Ceiling((double)totalItems / attr.PageSize);

            var prev = attr.Page > 0
                ? CreatePagingLink(attr.Page - 1, attr.PageSize, nameof(SearchQuestion))
                : null;
            var next = attr.Page < numberOfPages - 1
                ? CreatePagingLink(attr.Page + 1, attr.PageSize, nameof(SearchQuestion))
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

        private object CreateResult(IEnumerable<Answer> answers, int questionId, PagingAttributes attr)
        {
            var totalItems = _questionRepository.NoOfAnswers(questionId);
            var numberOfPages = Math.Ceiling((double)totalItems / attr.PageSize);

            var prev = attr.Page > 0
                ? CreatePagingLink(attr.Page - 1, attr.PageSize, nameof(GetAnswersForQuestion))
                : null;
            var next = attr.Page < numberOfPages - 1
                ? CreatePagingLink(attr.Page + 1, attr.PageSize, nameof(GetAnswersForQuestion))
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

        private AnswerDto CreateAnswerDto(Answer answer)
        {
            var dto = _mapper.Map<AnswerDto>(answer);
            dto.Link = Url.Link(
                    nameof(GetAnswersForQuestion),
                    new { answerId = answer.SubmissionId });
            return dto;
        }

        private string CreatePagingLink(int page, int pageSize, string str)
        {
            return Url.Link(str, new { page, pageSize });
        }
    }
}

