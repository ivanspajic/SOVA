using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2._Data_Layer_Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace _1._SOVA.Controllers
{
    [ApiController]
    [Route("api/{questionId}/answer")]
    public class AnswersController : ControllerBase
    {
        private readonly IAnswerRepository _answerRepository;

        public AnswersController(IAnswerRepository answerRepository)
        {
            _answerRepository = answerRepository;
        }

        [HttpGet(Name = nameof(GetAnswersForQuestion))]
        public ActionResult GetAnswersForQuestion(int questionId)
        {
            var answers = _answerRepository.GetAnswersForQuestionById(questionId);
            return Ok(answers);
        }
    }
}