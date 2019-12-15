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
    [Route("api/questions")]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionRepository _questionRepository;
        private IMapper _mapper;

        private readonly IAnswerRepository _answerRepository;


        public QuestionsController(IQuestionRepository questionRepository, IAnswerRepository answerRepository, IMapper mapper)
        {
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _mapper = mapper;
        }

        [HttpGet(Name = nameof(GetQuestions))]
        public ActionResult GetQuestions([FromQuery] PagingAttributes pagingAttributes)
        {
            var questions = _questionRepository.GetQuestions(pagingAttributes);
            return Ok(CreateResult(questions, pagingAttributes));
        }

        [HttpGet("{questionId}", Name = nameof(GetQuestionById))]
        public ActionResult GetQuestionById(int questionId)
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
            var answers = _answerRepository.GetAnswersForQuestionById(questionId);
            return Ok(CreateAnswerResult(answers, questionId, pagingAttributes));
        }

        [Authorize]
        [HttpGet("query/{queryString}", Name = nameof(SearchQuestion))]
        public ActionResult SearchQuestion([FromQuery] PagingAttributes pagingAttributes, string queryString)
        {
            int.TryParse(HttpContext.User.Identity.Name, out var userId);
            var searchResults = _questionRepository.SearchQuestions(queryString, userId, pagingAttributes);
            return Ok(CreateSearchResult(searchResults, queryString, userId, nameof(SearchQuestion), pagingAttributes));
        }

        [HttpGet("query/no-user/{queryString}", Name = nameof(SearchQuestionNoUser))]
        public ActionResult SearchQuestionNoUser([FromQuery] PagingAttributes pagingAttributes, string queryString)
        {
            var userId = 1;
            var searchResults = _questionRepository.SearchQuestions(queryString, userId, pagingAttributes);
            return Ok(CreateSearchResult(searchResults, queryString, userId, nameof(SearchQuestionNoUser), pagingAttributes));
        }

        [HttpGet("wordcloud/{queryString}", Name = nameof(GetCloudElements))]
        public ActionResult GetCloudElements(string queryString)
        {
            var userId = int.TryParse(HttpContext.User.Identity.Name, out var id) ? id : 1;
            var cloudElements = _questionRepository.GetWord2Words(queryString, userId);
            return Ok(CreateCloudElement(cloudElements));
        }

        [HttpGet("tag/{tagString}", Name = nameof(SearchQuestionByTag))]
        public ActionResult SearchQuestionByTag([FromQuery] PagingAttributes pagingAttributes, string tagString)
        {
            var searchResults = _questionRepository.GetQuestionsByTags(tagString, pagingAttributes);
            return Ok(CreateTagsResult(searchResults, tagString, pagingAttributes));
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
                    nameof(GetQuestionById),
                    new { questionId = question.SubmissionId });

            //handling comments and submission
            dto.Comments = question.Submission.Comments;
            dto.Submission.Comments = null; // this ensures we don't have duplicate comment collections, since we have a direct property already containing comments

            //handling linked posts
            dto.LinkPosts = question.LinkedPosts;

            //handling tags
            dto.Tags = question.QuestionsTags;
            dto.SoMember = question.Submission.SoMember;

            return dto;
        }

        private object CreateResult(IEnumerable<Question> questions, PagingAttributes attr)
        {
            var totalItems = _questionRepository.NoOfRandomQuestions();
            var numberOfPages = Math.Ceiling((double)totalItems / attr.PageSize);

            var prev = attr.Page > 0
                ? CreatePagingLink(attr.Page - 1, attr.PageSize, nameof(GetQuestions))
                : null;
            var next = attr.Page < numberOfPages - 1
                ? CreatePagingLink(attr.Page + 1, attr.PageSize, nameof(GetQuestions))
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

        private object CreateSearchResult(IEnumerable<SearchResult> questions, string str, int userId, string name, PagingAttributes attr)
        {
            var totalItems = _questionRepository.NoOfResults(str, userId);
            var numberOfPages = Math.Ceiling((double)totalItems / attr.PageSize);

            var prev = attr.Page > 0
                ? CreatePagingLink(attr.Page - 1, attr.PageSize, name)
                : null;
            var next = attr.Page < numberOfPages - 1
                ? CreatePagingLink(attr.Page + 1, attr.PageSize, name)
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

        private object CreateCloudElement(IEnumerable<CloudElement> elements)
        {
            return new
            {
                items = elements
            };
        }

        private object CreateTagsResult(IEnumerable<QuestionsTag> questions, string str, PagingAttributes attr)
        {
            var totalItems = _questionRepository.NoOfResultsForTag(str, null);
            var numberOfPages = Math.Ceiling((double)totalItems / attr.PageSize);

            var prev = attr.Page > 0
                ? CreatePagingLink(attr.Page - 1, attr.PageSize, nameof(SearchQuestionByTag))
                : null;
            var next = attr.Page < numberOfPages - 1
                ? CreatePagingLink(attr.Page + 1, attr.PageSize, nameof(SearchQuestionByTag))
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


        private object CreateAnswerResult(IEnumerable<Answer> answers, int questionId, PagingAttributes attr)
        {
            var totalItems = _answerRepository.NoOfAnswers(questionId); // workaround
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

