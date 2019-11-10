using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2._Data_Layer_Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace _1._SOVA.Controllers
{
    [ApiController]
    [Route("api/history")]
    public class HistoriesController : ControllerBase
    {
        private readonly IHistoryRepository _historyRepository;
        private IMapper _mapper;

        public HistoriesController(IHistoryRepository historyRepository, IMapper mapper)
        {
            _historyRepository = historyRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetHistoryForUser()
        {
            return Ok();
        }
    }
}