using AutoMapper;
using Data_Layer_Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using SOVA.Models;
using System.Collections.Generic;

namespace SOVA.Controllers
{
    [ApiController]
    [Route("api/history")]
    public class HistoriesController : ControllerBase
    {
        private readonly IUserHistoryRepository _userHistoryRepository;
        private IMapper _mapper;

        public HistoriesController(IUserHistoryRepository userHistoryRepository, IMapper mapper)
        {
            _userHistoryRepository = userHistoryRepository;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet(Name = nameof(GetUserHistoryByUserId))]
        public IActionResult GetUserHistoryByUserId()
        {
            int.TryParse(HttpContext.User.Identity.Name, out var userId);
            var history = _userHistoryRepository.GetUserHistoryByUserId(userId);
            if (history == null)
            {
                return NoContent();
            }
            return Ok(CreateResult(history, userId));
        }
        ///////////////////
        //
        // Helpers
        //
        ///////////////////

        private UserHistoryDto CreateUserHistroyDto(UserHistory userHistory)
        {
            var dto = _mapper.Map<UserHistoryDto>(userHistory);
            dto.Link = Url.Link(
            nameof(GetUserHistoryByUserId),
            new { userId = userHistory.UserId, historyId = userHistory.HistoryId });
            return dto;
        }

        private object CreateResult(IEnumerable<UserHistory> userHistories, int userId)
        {
            var totalItems = _userHistoryRepository.NoOfUserHistory(userId);
            return new
            {
                totalItems,
                items = userHistories
            };
        }
    }
}