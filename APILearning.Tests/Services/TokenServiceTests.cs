using APILearning.Entities;
using APILearning.Services.Implement;
using APILearning.Services.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Moq.Microsoft.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace APILearning.Tests.Services
{
    public class TokenServiceTests
    {

        [Fact]
        public void CreateToken_ReturnsValidToken()
        {
            // Arrange
            Mock<IConfiguration> mockConfig = new Mock<IConfiguration>();
            mockConfig.SetupConfiguration().Returns(new
            {
                TokenKey = "super secret unguessable key"
            });
            //mockConfig.Setup(config => config.GetValue<string>("TokenKey")).Returns("super secret unguessable key");

            TokenService _tokenService = new TokenService(mockConfig.Object);
            var user = new AppUser
            {
                Id = 1,
                Email = "example@example.com"
            };

            // Act
            var token = _tokenService.CreateToken(user);

            // Assert
            Assert.NotNull(token);
            Assert.NotEmpty(token);

            // You can add additional assertions to validate the token contents if needed
            var tokenHandler = new JwtSecurityTokenHandler();
            var decodedToken = tokenHandler.ReadJwtToken(token);
            Assert.Equal(user.Id.ToString(), decodedToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.NameId)?.Value);
            Assert.Equal(user.Email, decodedToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName)?.Value);
        }
    }
}