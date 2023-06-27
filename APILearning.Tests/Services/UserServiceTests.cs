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

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<AppUser>(registerDto)).Returns(appUser);
            mapperMock.Setup(mapper => mapper.Map<UserDto>(appUser)).Returns(userDto);

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repository => repository.AddAsync(appUser)).Verifiable();

            var tokenServiceMock = new Mock<ITokenService>();

            var userService = new UserService(mapperMock.Object, userRepositoryMock.Object, tokenServiceMock.Object);

            // Act
            var result = await userService.Register(registerDto);

            // Assert
            Assert.Equal(userDto, result);
            userRepositoryMock.Verify(repository => repository.AddAsync(appUser), Times.Once);
        }


    }
}
