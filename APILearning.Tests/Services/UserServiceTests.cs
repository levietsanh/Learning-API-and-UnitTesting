using APILearning.Data;
using APILearning.DTOs;
using APILearning.Entities;
using APILearning.Repositories.Interface;
using APILearning.Services.Implement;
using APILearning.Services.Interface;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace APILearning.Tests.Services
{
    public class UserServiceTests
    {
        private readonly UserService _userService;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        public UserServiceTests()
        {
            _mapperMock = new Mock<IMapper>();
            _tokenServiceMock = new Mock<ITokenService>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _userService = new UserService(_mapperMock.Object, _userRepositoryMock.Object, _tokenServiceMock.Object);
        }

        [Fact]
        public async Task Register_Should_Return_UserDto()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Email = "test@example.com",
                Password = "password"
                // Add any other properties required for registration
            };

            var appUser = new AppUser
            {
                // Set any required properties for AppUser
            };

            var userDto = new UserDto
            {
                // Set the expected properties for UserDto
            };

            
            _mapperMock.Setup(mapper => mapper.Map<AppUser>(registerDto)).Returns(appUser);
            _mapperMock.Setup(mapper => mapper.Map<UserDto>(appUser)).Returns(userDto);

            
            _userRepositoryMock.Setup(repository => repository.AddAsync(appUser)).Verifiable();

            

            // Act
            var result = await _userService.Register(registerDto);

            // Assert
            Assert.Equal(userDto, result);
            _userRepositoryMock.Verify(repository => repository.AddAsync(appUser), Times.Once);
        }


        //Login
        [Fact]
        public async Task Login_ValidCredentials_ReturnsUserDto()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "test@example.com",
                Password = "password123"
            };
            using var hmac = new HMACSHA512();
            var user = new AppUser
            {
                Email = "test@example.com",
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password)),
                PasswordSalt = hmac.Key,
                Address = "123 Test St",
                Gender = "Male"
            };

            _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(loginDto.Email))
                .ReturnsAsync(user);

            _tokenServiceMock.Setup(service => service.CreateToken(user))
                .Returns("super secret unguessable key");

            // Act
            var result = await _userService.Login(loginDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(loginDto.Email, result.Email);
            Assert.Equal("super secret unguessable key", result.Token);
            Assert.Equal(user.Address, result.Address);
            Assert.Equal(user.Gender, result.Gender);
        }
        [Fact]
        public async Task Login_WithInvalidEmail_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "invalid@example.com",
                Password = "password"
            };

            _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(loginDto.Email)).ReturnsAsync((AppUser)null);

            // Act and Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _userService.Login(loginDto));
        }

        [Fact]
        public async Task Login_WithInvalidPassword_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "test@example.com",
                Password = "invalid_password"
            };

            var user = new AppUser
            {
                Email = "test@example.com",
                PasswordSalt = new byte[64], // Set appropriate password salt and hash values
                PasswordHash = new byte[64]
            };

            _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(loginDto.Email)).ReturnsAsync(user);

            // Act and Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _userService.Login(loginDto));
        }
    }
}
