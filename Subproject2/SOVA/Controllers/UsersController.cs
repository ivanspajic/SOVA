using AutoMapper;
using Data_Layer_Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using SOVA.Models;
using System.Collections.Generic;
using System.Linq;

namespace SOVA.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet(Name = nameof(GetUsers))]
        public ActionResult GetUsers()
        {
            var users = _userRepository.GetUsers();
            return Ok(CreateResult(users));
        }

        [HttpGet("{userId}", Name = nameof(GetUserById))]
        public ActionResult GetUserById(int userId)
        {
            var userById = _userRepository.GetUserById(userId);
            if (userById == null)
            {
                return NotFound($"Not found. UserId: '{userId}'");
            }
            return Ok(CreateUserDto(userById));
        }

        [Authorize]
        [HttpGet("currentUser", Name = nameof(GetCurrentUser))]
        public ActionResult GetCurrentUser()
        {
            int.TryParse(HttpContext.User.Identity.Name, out var userId);
            var userById = _userRepository.GetUserById(userId);
            return userById == null ? Ok("Not authorized.") : Ok(CreateUserDto(userById));
        }
        ///////////////////
        //
        // Helpers
        //
        ///////////////////
        private UserDto CreateUserDto(User user)
        {
            var dto = _mapper.Map<UserDto>(user);
            dto.Link = Url.Link(
                nameof(GetUsers),
                new
                {
                    userId = user.Id
                });
            return dto;
        }
        private IEnumerable<UserDto> CreateResult(IEnumerable<User> users)
        {
            return users.Select(u => CreateUserDto(u));
        }
    }
}