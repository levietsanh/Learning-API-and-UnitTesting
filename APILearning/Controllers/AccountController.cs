using APILearning.Data;
using APILearning.DTOs;
using APILearning.Entities;
using APILearning.Services.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace APILearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AccountController( IUserService accountService, ITokenService tokenService)
        {
            _userService = accountService;
            _tokenService = tokenService;
        }

        //Register
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
           if(await _userService.CheckUserIfExists(registerDto.Email))
            {
                return BadRequest("Email is taken");
            }
            var userDto = await _userService.Register(registerDto);

            return Ok(userDto);
        }
        /*
        //Login
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Login(string email, string password)
        {
            //if (await _userService.UserExists(registerDto.Email))
            //{
            //    return BadRequest("Email is taken");
            //}
            //var user = await _userService.Register(registerDto);
            //var userDto = _mapper.Map<UserDto>(user);

            //return Ok(userDto);
            if (await _userService.CheckUserIfExists(email))
            {
                AppUser user = await _userService.DoLogin(email, password);
                if (user != null)
                {
                    var token = _tokenService.CreateToken(user);
                    return Ok(token);
                }
                else
                {
                    return NotFound("");
                }
            }
            else
            {
                return BadRequest(".....");
            }
        }
        */
        //Login
        /*
         [HttpPost("login")]
         public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
         {
             var user = await _context.Users
                                 .SingleOrDefaultAsync(x => x.Email == loginDto.Email);
             if (user == null)
             {
                 return Unauthorized("Invalid Email");
             }
             using var hmac = new HMACSHA512(user.PasswordSalt);
             var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
             for ( int i=0; i< computedHash.Length; i++ )
             {
                 if (computedHash[i] != user.PasswordHash[i])
                     return Unauthorized("Invalid Password");
             }
             return new UserDto
             {
                 Email = loginDto.Email,
                 Token = _tokenService.CreateToken(user),
                 Address = user.Address,
                 Gender = user.Gender,
             };
         }
        */
        
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login (LoginDto loginDto)
        {
            try
            {
                var userDto = await _userService.Login(loginDto);
                return Ok(userDto);
            }
            catch(UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
        

    }
}
