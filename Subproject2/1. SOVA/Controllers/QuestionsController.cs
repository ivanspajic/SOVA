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

        //[HttpGet("{queryString}", Name = nameof(SearchQuestion))]
        //public ActionResult SearchQuestion(string queryString)
        //{
        //    var searchResults = _questionRepository.SearchQuestions(queryString);
        //    return Ok(searchResults);
        //}

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
                    new { questionId = question.Submission.Id });
            return dto;
        }

        private IEnumerable<QuestionDto> CreateResult(IEnumerable<Question> questions)
        {
            return questions.Select(q => CreateQuestionDto(q));
        }
    }
}

