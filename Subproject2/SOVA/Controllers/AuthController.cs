using AutoMapper;
using Data_Layer_Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SOVA.Models;
using SOVA.Service;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SOVA.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private IMapper _mapper;
        private int _size;
        public AuthController(IUserRepository userRepository, IConfiguration configuration, IMapper mapper)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _mapper = mapper;
            GetAuthPasswordSizeFromConfig();
        }

        [HttpPost("users")]
        public ActionResult CreateUser([FromBody] UserForCreation dto)
        {
            if (dto.Username == null || dto.Password == null)
            {
                Console.WriteLine("am I here?");
                return BadRequest(new { message = "Please fill out all fields." });
            }
            if (_userRepository.GetUserByUsername(dto.Username) != null)
            {
                return BadRequest(new { message = "Username is already taken. Please choose another username." });
            }

            var salt = PasswordService.GenerateSalt(_size);
            var pwd = PasswordService.HashPassword(dto.Password, salt, _size);
            _userRepository.CreateUser(dto.Username, pwd, salt);
            return CreatedAtRoute(null, new { username = dto.Username });
        }

        [HttpPatch("users/{userId}")]
        public ActionResult UpdateUserById(int userId, [FromBody] UserForUpdate dto)
        {
            if (_userRepository.GetUserById(userId) == null)
            {
                return BadRequest();
            }

            var updatedUsername = dto.Username;
            // Check if username is already taken in database.
            if (_userRepository.GetUserByUsername(dto.Username) != null && _userRepository.GetUserByUsername(dto.Username).Username != dto.Username)
            {
                return BadRequest(new { message = "Username is already taken. Please choose another username." });
            }

            var updatedSalt = dto.Password != null ? PasswordService.GenerateSalt(_size) : null;
            var updatedPassword = dto.Password != null ? PasswordService.HashPassword(dto.Password, updatedSalt, _size) : null;
            _userRepository.UpdateUser(userId, updatedUsername, updatedPassword, updatedSalt);

            return Ok(dto.Username);
        }


        [HttpPost("tokens")]
        public ActionResult Login([FromBody] UserDto dto)
        {
            var user = _userRepository.GetUserByUsername(dto.Username);
            if (user == null)
            {
                return BadRequest(new { message = "Username not found." });
            }
            var pwd = PasswordService.HashPassword(dto.Password, user.Salt, _size);

            if (user.Password != pwd)
            {
                return BadRequest(new { message = "Wrong password." });
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Auth:Key"]);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var securityToken = tokenHandler.CreateToken(tokenDescription);
            var token = tokenHandler.WriteToken(securityToken);
            return Ok(new { user.Username, token });
        }

        private void GetAuthPasswordSizeFromConfig()
        {
            int.TryParse(
                _configuration.GetSection("Auth:PwdSize").Value,
                out _size);

            if (_size == 0)
            {
                throw new ArgumentException();
            }
        }
    }
}