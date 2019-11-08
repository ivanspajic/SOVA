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
    [Route("api/{questionId}/answer")]
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
        public ActionResult GetAnswersForQuestion(int questionId)
        {
            var answers = _answerRepository.GetAnswersForQuestionById(questionId);
            return Ok(CreateResult(answers));
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
    }
}