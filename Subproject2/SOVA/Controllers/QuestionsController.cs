﻿using System;
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
    [Route("api/questions")]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionRepository _questionRepository;
        private IMapper _mapper;

        private readonly IAnswerRepository _answerRepository;
        private readonly ICommentRepository _commentRepository;

        public QuestionsController(IQuestionRepository questionRepository, IAnswerRepository answerRepository, ICommentRepository commentRepository, IMapper mapper)
        {
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _commentRepository = commentRepository;
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

        //[Authorize]
        [HttpGet("query/{queryString}", Name = nameof(SearchQuestion))]
        public ActionResult SearchQuestion([FromQuery] PagingAttributes pagingAttributes, string queryString)
        {
            var userId = int.TryParse(HttpContext.User.Identity.Name, out var id) ? id : 1;
            var searchResults = _questionRepository.SearchQuestions(queryString, userId, pagingAttributes);
            return Ok(CreateSearchResult(searchResults, queryString, userId, pagingAttributes));
        }

        //[Authorize]
        [HttpGet("tag/{tagString}", Name = nameof(SearchQuestionByTag))]
        public ActionResult SearchQuestionByTag([FromQuery] PagingAttributes pagingAttributes, string tagString)
        {
            var userId = int.TryParse(HttpContext.User.Identity.Name, out var id) ? id : 1;
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
            dto.Answers = _answerRepository.GetAnswersForQuestionById(question.SubmissionId);
            dto.Comments = _commentRepository.GetAllCommentsBySubmissionId(question.SubmissionId);
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

        private object CreateSearchResult(IEnumerable<SearchResult> questions, string str, int userId, PagingAttributes attr)
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
        private object CreateTagsResult(IEnumerable<QuestionsTag> questions, string str, PagingAttributes attr)
        {
            var totalItems = _questionRepository.NoOfResults(str, null);
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
