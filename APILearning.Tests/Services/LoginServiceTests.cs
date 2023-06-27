using APILearning.DTOs;
using APILearning.Entities;
using APILearning.Repositories.Interface;
using APILearning.Services.Implement;
using APILearning.Services.Interface;
using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APILearning.Tests.Services
{
    public class LoginServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly Mock<IMapper> _mapper;
        private readonly UserService _loginService;

        public LoginServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _tokenServiceMock = new Mock<ITokenService>();
            _mapper = new Mock<IMapper>();
            _loginService = new UserService(_mapper.Object,_userRepositoryMock.Object, _tokenServiceMock.Object);
        }

        [Fact]
        public async Task Login_WithValidCredentials_ReturnsUserDto()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "test@example.com",
                Password = "password"
            };

            var user = new AppUser
            {
                Email = "test@example.com",
                PasswordSalt = new byte[64], // Set appropriate password salt and hash values
                PasswordHash = new byte[64]
            };

            _userRepositoryMock.Setup(repo => repo.GetUsersByEmailAsync(loginDto.Email)).ReturnsAsync(user);
            _tokenServiceMock.Setup(service => service.CreateToken(user)).Returns("super secret unguessable key");

            // Act
            var result = await _loginService.Login(loginDto);

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

            _userRepositoryMock.Setup(repo => repo.GetUsersByEmailAsync(loginDto.Email)).ReturnsAsync((AppUser)null);

            // Act and Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _loginService.Login(loginDto));
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

            _userRepositoryMock.Setup(repo => repo.GetUsersByEmailAsync(loginDto.Email)).ReturnsAsync(user);

            // Act and Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _loginService.Login(loginDto));
        }
    }
}
