using APILearning.DTOs;
using APILearning.Entities;

namespace APILearning.Services.Interface
{
    public interface IUserService
    {
        Task<UserDto> Register(RegisterDto registerDto);
        Task<bool> CheckUserIfExists(string email);
        Task<UserDto>Login(LoginDto loginDto);


        
    }
}
