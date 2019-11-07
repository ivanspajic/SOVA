using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2._Data_Layer_Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace _1._SOVA.Controllers
{
    [ApiController]
    [Route("api/Histories")]
    public class HistoriesController : ControllerBase
    {
        private readonly IHistoryRepository historyRepository;

        public HistoriesController(IHistoryRepository historyRepository)
        {
            this.historyRepository = historyRepository;
        }

        [HttpGet]
        public IActionResult GetHistoryForUser()
        {
            return Ok();
        }
    }
}