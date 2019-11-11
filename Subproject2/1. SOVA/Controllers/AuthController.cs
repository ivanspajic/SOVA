using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using _1._SOVA;
using _1._SOVA.Models;
using _2._Data_Layer_Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace WebServiceToken.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthController(IUserRepository userRepository, IConfiguration configuration, IMapper mapper)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpPost("users")]
        public ActionResult CreateUser([FromBody] UserForCreation dto)
        {
            if (_userRepository.GetUserByUsername(dto.Username) != null)
            {
                return BadRequest();
            }

            int.TryParse(
                _configuration.GetSection("Auth:PwdSize").Value,
                out var size);

            if (size == 0)
            {
                throw new ArgumentException();
            }

            var salt = PasswordService.GenerateSalt(size);

            var pwd = PasswordService.HashPassword(dto.Password, salt, size);

            _userRepository.CreateUser(dto.Username, pwd, salt);

            return CreatedAtRoute(null, dto.Username);
        }

        [HttpPost("tokens")]
        public ActionResult Login([FromBody] UserDto dto)
        {
            var user = _userRepository.GetUserByUsername(dto.Username);

            if (user == null)
            {
                return BadRequest();
            }

            int.TryParse(
                _configuration.GetSection("Auth:PwdSize").Value,
                out var size);

            if (size == 0)
            {
                throw new ArgumentException();
            }

            var pwd = PasswordService.HashPassword(dto.Password, user.Salt, size);

            if (user.Password != pwd)
            {
                return BadRequest();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Auth:Key"]);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                }),
                Expires = DateTime.Now.AddSeconds(20),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var securityToken = tokenHandler.CreateToken(tokenDescription);

            var token = tokenHandler.WriteToken(securityToken);

            return Ok(new { user.Username, token });

        }

    }
}