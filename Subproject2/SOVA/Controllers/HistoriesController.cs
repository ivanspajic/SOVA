using System;
using System.Collections.Generic;
using AutoMapper;
using Data_Layer_Abstractions;
using Microsoft.AspNetCore.Mvc;
using Models;
using SOVA.Models;

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

        //[Authorize]
        [HttpGet(Name = nameof(GetUserHistoryByUserId))]
        public IActionResult GetUserHistoryByUserId([FromQuery] PagingAttributes pagingAttributes)
        {
            var userId = int.TryParse(HttpContext.User.Identity.Name, out var id) ? id : 39;
            var history = _userHistoryRepository.GetUserHistoryByUserId(userId, pagingAttributes);
            if (history == null)
            {
                return NotFound();
            }
            return Ok(CreateResult(history, userId, pagingAttributes));
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

        private object CreateResult(IEnumerable<UserHistory> userHistories, int userId, PagingAttributes attr)
        {
            var totalItems = _userHistoryRepository.NoOfUserHistory(userId);
            var numberOfPages = Math.Ceiling((double)totalItems / attr.PageSize);

            var prev = attr.Page > 0
                ? CreatePagingLink(attr.Page - 1, attr.PageSize)
                : null;
            var next = attr.Page < numberOfPages - 1
                    ? CreatePagingLink(attr.Page + 1, attr.PageSize)
                    : null;

            return new
            {
                totalItems,
                numberOfPages,
                prev,
                next,
                items = userHistories
            };
        }

        private string CreatePagingLink(int page, int pageSize)
        {
            return Url.Link(nameof(GetUserHistoryByUserId), new { page, pageSize });
        }

    }
}