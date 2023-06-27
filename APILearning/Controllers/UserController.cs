using APILearning.Entities;
using APILearning.Repositories.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APILearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        //Get Users
        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            var users = await _userRepository.GetUsersAsync();
            return Ok(users);
        }
        //Get User by ID
        [HttpGet("users/{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {          
            return await _userRepository.GetUserByIdAsync(id);
        }
    }
}
