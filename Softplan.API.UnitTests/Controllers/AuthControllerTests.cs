using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace Softplan.API.UnitTests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Softplan.API.Presentation.Controllers.AuthController _controller;

        public AuthControllerTests()
        {
            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.Setup(c => c["Jwt:Key"]).Returns("YourSuperSecretKeyHere12345678901234567890");
            _configurationMock.Setup(c => c["Jwt:Issuer"]).Returns("Softplan.API");
            _configurationMock.Setup(c => c["Jwt:Audience"]).Returns("Softplan.API");

            _controller = new Softplan.API.Presentation.Controllers.AuthController(_configurationMock.Object);
        }

        [Fact]
        public void Login_WithValidCredentials_ReturnsOkWithToken()
        {
            // Arrange
            var request = new Softplan.API.Presentation.Controllers.LoginRequest
            {
                Username = "admin",
                Password = "password"
            };

            // Act
            var result = _controller.Login(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);

            var tokenProperty = okResult.Value.GetType().GetProperty("Token");
            Assert.NotNull(tokenProperty);
            var token = tokenProperty.GetValue(okResult.Value) as string;
            Assert.NotNull(token);
            Assert.NotEmpty(token);
        }

        [Fact]
        public void Login_WithInvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var request = new Softplan.API.Presentation.Controllers.LoginRequest
            {
                Username = "admin",
                Password = "wrongpassword"
            };

            // Act
            var result = _controller.Login(request);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public void Login_WithWrongUsername_ReturnsUnauthorized()
        {
            // Arrange
            var request = new Softplan.API.Presentation.Controllers.LoginRequest
            {
                Username = "wronguser",
                Password = "password"
            };

            // Act
            var result = _controller.Login(request);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public void GenerateJwtToken_CreatesValidToken()
        {
            // Arrange
            var username = "testuser";

            // Act
            var token = typeof(Softplan.API.Presentation.Controllers.AuthController)
                .GetMethod("GenerateJwtToken", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(_controller, new object[] { username }) as string;

            // Assert
            Assert.NotNull(token);
            Assert.NotEmpty(token);

            // Verify token can be read
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            Assert.Equal("Softplan.API", jwtToken.Issuer);
            Assert.Equal("Softplan.API", jwtToken.Audiences.First());
            Assert.Contains(jwtToken.Claims, c => c.Type == JwtRegisteredClaimNames.Sub && c.Value == username);
            Assert.Contains(jwtToken.Claims, c => c.Type == JwtRegisteredClaimNames.Jti);
        }

        [Fact]
        public void GenerateJwtToken_ThrowsException_WhenJwtKeyNotConfigured()
        {
            // Arrange
            var configMock = new Mock<IConfiguration>();
            configMock.Setup(c => c["Jwt:Key"]).Returns((string)null!);
            var controller = new Softplan.API.Presentation.Controllers.AuthController(configMock.Object);

            // Act & Assert
            var exception = Assert.Throws<System.Reflection.TargetInvocationException>(() =>
                typeof(Softplan.API.Presentation.Controllers.AuthController)
                    .GetMethod("GenerateJwtToken", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?.Invoke(controller, new object[] { "testuser" })
            );

            Assert.IsType<InvalidOperationException>(exception.InnerException);
            Assert.Contains("JWT Key is not configured", exception.InnerException?.Message);
        }
    }
}