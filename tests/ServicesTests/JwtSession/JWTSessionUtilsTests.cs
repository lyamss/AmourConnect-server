using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Services;
using Domain.Utils;
using Moq;
using Microsoft.Extensions.Options;
using Domain.Dtos.AppLayerDtos;
using Microsoft.AspNetCore.Http;
namespace Tests.Application.ServicesTests.JwtSession
{
    public class JWTSessionUtilsTests
    {
        private readonly Mock<IOptions<SecretEnv>> _mockJwtSecretOptions;
        private readonly JWTSessionUtils _jwtSessionUtils;

        public JWTSessionUtilsTests()
        {
            _mockJwtSecretOptions = new Mock<IOptions<SecretEnv>>();
            _mockJwtSecretOptions.Setup(x => x.Value).Returns(new SecretEnv
            {
                SecretKeyJWT = "sdzwqsdcszedswqsazdfcdxswqszdcfg",
                Ip_Now_Frontend = "http://frontend",
                Ip_Now_Backend = "http://backend"
            });

            _jwtSessionUtils = new JWTSessionUtils(_mockJwtSecretOptions.Object);
        }

        [Fact]
        public void GenerateJwtToken_ShouldReturnValidToken()
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "test-user"),
                new Claim(ClaimTypes.Role, "admin")
            };
            var expirationValue = DateTime.UtcNow.AddMinutes(30);

            SessionUserDto token = _jwtSessionUtils.GenerateJwtToken(claims, expirationValue);

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token.token_session_user);

            Assert.Equal("test-user", jwtToken.Claims.First(c => c.Type == ClaimTypes.Name).Value);
            Assert.Equal("admin", jwtToken.Claims.First(c => c.Type == ClaimTypes.Role).Value);
            Assert.Equal("http://frontend", jwtToken.Issuer);
            Assert.Equal("http://backend", jwtToken.Audiences.Single());
            Assert.True(jwtToken.ValidTo > DateTime.UtcNow);
        }

        [Fact]
        public void SetSessionCookie_ShouldAppendCookieWithCorrectOptions()
        {
            var response = new Mock<HttpResponse>();
            var cookieCollection = new Mock<IResponseCookies>();
            var sessionData = new SessionUserDto
            {
                token_session_user = "testToken",
                date_token_session_expiration = DateTime.UtcNow.AddDays(1)
            };

            response.Setup(r => r.Cookies).Returns(cookieCollection.Object);

            _jwtSessionUtils.SetSessionCookie(response.Object, "testCookie", sessionData);

            cookieCollection.Verify(c => c.Append("testCookie", "testToken", It.IsAny<CookieOptions>()), Times.Once);
        }
    }
}
