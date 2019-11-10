using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using _0._Models;
using _1._SOVA.Models;
using _2._Data_Layer_Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;


namespace _1._SOVA.Controllers
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
        ///////////////////
        //
        // Helpers
        //
        ///////////////////
        private UserForLoginDto CreateUserDto(User user)
        {
            var dto = _mapper.Map<UserForLoginDto>(user);
            dto.Link = Url.Link(
                nameof(GetUsers),
                new
                {
                    userId = user.Id
                });
            return dto;
        }
        private IEnumerable<UserForLoginDto> CreateResult(IEnumerable<User> users)
        {
            return users.Select(u => CreateUserDto(u));
        }

    }

}