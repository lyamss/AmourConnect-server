using Domain.Utils;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using Application.Interfaces.Services;
using Microsoft.Extensions.Options;
using Domain.Dtos.AppLayerDtos;
using Microsoft.AspNetCore.Http;
namespace Application.Services
{
    public sealed class JWTSessionUtils(IOptions<SecretEnv> jwtSecret) : IJWTSessionUtils
    {
        public string NameCookieUserConnected { get; } = "User-AmourConnect";
        public string NameCookieUserGoogle { get; } = "GoogleUser-AmourConnect";


        private readonly SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(jwtSecret.Value.SecretKeyJWT));
        private readonly string ip_Now_Frontend = jwtSecret.Value.Ip_Now_Frontend;
        private readonly string ip_Now_Backend = jwtSecret.Value.Ip_Now_Backend;

        public SessionUserDto GenerateJwtToken(Claim[] claims, DateTime expirationValue)
        {
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                ip_Now_Frontend,
                ip_Now_Backend,
                claims,
                expires: expirationValue,
                signingCredentials: credentials
            );

            string jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return new SessionUserDto()
            {
                token_session_user = jwt,
                date_token_session_expiration = expirationValue,
            };
        }

        public void SetSessionCookie(HttpResponse Response, string nameOfCookie, SessionUserDto sessionData)
        {
            DateTimeOffset dateExpiration = sessionData.date_token_session_expiration;
            DateTimeOffset currentDate = DateTimeOffset.UtcNow;
            TimeSpan maxAge = dateExpiration - currentDate;

            Response.Cookies.Append(
                nameOfCookie,
                sessionData.token_session_user,
                new CookieOptions
                {
                    Path = "/",
                    MaxAge = maxAge,
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                    Secure = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production"
                }
            );
        }

        public IEnumerable<Claim> GetClaimsFromCookieJWT(HttpContext httpContext, string nameOfCookie)
        {
            string jwt = GetCookie(httpContext, nameOfCookie);

            var handler = new JwtSecurityTokenHandler();

            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production",
                    ValidateAudience = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production",
                    RequireExpirationTime = false,
                    ValidateLifetime = true,
                };

                var principal = handler.ValidateToken(jwt, tokenValidationParameters, out var validatedToken);
                var claims = principal.Claims;

                if (claims == null)
                    return null;

                return claims;
            }
            catch
            {
                return null;
            }
        }

        public string GetValueClaimsCookieUser(HttpContext httpContext)
        {
            var claims = GetClaimsFromCookieJWT(httpContext, NameCookieUserConnected);

            string userC = claims?.FirstOrDefault(c => c.Type == "userAmourConnected")?.Value;

            if (userC == null)
                return null;

            string cookieValue = GetCookie(httpContext, NameCookieUserConnected);

            return cookieValue;
        }

        public string GetCookie(HttpContext httpContext, string nameOfCookie)
        {
            if (!httpContext.Request.Cookies.TryGetValue(nameOfCookie, out string jwt))
            {
                return null;
            }
            return jwt;
        }
    }
}