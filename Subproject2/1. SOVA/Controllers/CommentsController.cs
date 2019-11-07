using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2._Data_Layer_Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace _1._SOVA.Controllers
{
    [ApiController]
    [Route("api/Answers")]
    public class CommentsController : ControllerBase
    {
        private readonly IHistoryRepository historyRepository;

        public CommentsController(IHistoryRepository historyRepository)
        {
            this.historyRepository = historyRepository;
        }

        [HttpGet]
        [Route("{userId}")]
        public IActionResult GetAnswerForUser(int userId)
        {
            return Ok(userId); //dummy code
        }
    }
}

