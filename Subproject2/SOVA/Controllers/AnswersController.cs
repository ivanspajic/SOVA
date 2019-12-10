using AutoMapper;
using Data_Layer_Abstractions;
using Microsoft.AspNetCore.Mvc;
using Models;
using SOVA.Models;

namespace SOVA.Controllers
{
    [ApiController]
    [Route("api/")]
    public class AnswersController : ControllerBase
    {
        private readonly IAnswerRepository _answerRepository;
        private IMapper _mapper;

        public AnswersController(IAnswerRepository answerRepository, IMapper mapper)
        {
            _answerRepository = answerRepository;
            _mapper = mapper;
        }

        [HttpGet("answers/{answerId}", Name = nameof(GetAnswerById))]
        public ActionResult GetAnswerById(int answerId)
        {
            var answers = _answerRepository.GetAnswerById(answerId);
            return Ok(CreateAnswerDto(answers));
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
                    nameof(GetAnswerById),
                    new { answerId = answer.SubmissionId});
            return dto;
        }
    }
}