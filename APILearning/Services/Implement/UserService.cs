using APILearning.Data;
using APILearning.DTOs;
using APILearning.Entities;
using APILearning.Repositories.Interface;
using APILearning.Services.Interface;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace APILearning.Services.Implement
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public UserService(IMapper mapper, IUserRepository userRepository, ITokenService tokenService)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

       

        public async Task<UserDto> Register(RegisterDto registerDto)
        {
            var user = _mapper.Map<AppUser>(registerDto);
            using var hmac = new HMACSHA512();
            user.Email = registerDto.Email;
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt = hmac.Key;

            await _userRepository.AddAsync(user);
            var userDto = _mapper.Map<UserDto>(user);

            return userDto;
        }

        public async Task<bool> CheckUserIfExists(string email)
        {
            return await _userRepository.CheckUserIfExist(email);
        }
        
        public async Task<UserDto> Login(LoginDto loginDto)
        {
            //var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == loginDto.Email);
            AppUser user = await _userRepository.GetUsersByEmailAsync(loginDto.Email);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid Email");
            }
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computerHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for ( int i=0; i<computerHash.Length; i++)
            {
                if (computerHash[i] != user.PasswordHash[i])
                    throw new UnauthorizedAccessException("Invalid Password");
            }
            return new UserDto
            {
                Email = loginDto.Email,
                Token = _tokenService.CreateToken(user),
                Address = user.Address,
                Gender = user.Gender,
            };
        }
        
    }
}
