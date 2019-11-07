﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        [HttpGet(Name = nameof(GetAnswerForQuestion))]
        public ActionResult GetAnswerForQuestion(int questionId)
        {
            var answers = _answerRepository.GetAnswerForQuestion(questionId);
            return Ok(answers);
        }
    }
}
