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
    [Route("api/{userId}/history")]
    public class HistoriesController : ControllerBase
    {
        private readonly IHistoryRepository _historyRepository;
        private IMapper _mapper;

        public HistoriesController(IHistoryRepository historyRepository, IMapper mapper)
        {
            _historyRepository = historyRepository;
            _mapper = mapper;
        }

        [HttpGet(Name = nameof(GetHistoryForUser))]
        public IActionResult GetHistoryForUser(int userId)
        {
            return Ok(_historyRepository.GetHistoryById(userId));
        }
    }
}