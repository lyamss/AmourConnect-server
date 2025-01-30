using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using API.Features.Authentification.Dtos;
using API.Entities;
using Microsoft.Extensions.Options;




namespace API.Services
{
    internal sealed class JWTSessionUtils(IOptions<SecretEnv> jwtSecret, IHttpContextAccessor httpContextAccessor, IRepository<User> repositoryU, StringConfig _stringConfig) : IJWTSessionUtils
    {
        private readonly SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(jwtSecret.Value.SecretKeyJWT));
        private readonly string ip_Now_Frontend = jwtSecret.Value.IpFrontend;
        private readonly string ip_Now_Backend = jwtSecret.Value.IP_Backend;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IRepository<User> _repositoryU = repositoryU;
        private readonly StringConfig stringConfig = _stringConfig;

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

            return new SessionUserDto(jwt, expirationValue);
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
            string jwt = this.GetCookie(httpContext, nameOfCookie);

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
            var claims = this.GetClaimsFromCookieJWT(httpContext, this.stringConfig.NameCookieUserConnected);

            string userC = claims?.FirstOrDefault(c => c.Type == this.stringConfig.NameClaimsJWTUser)?.Value;

            if (userC == null)
                return null;

            string cookieValue = this.GetCookie(httpContext, this.stringConfig.NameCookieUserConnected);

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


        public async Task CreateSessionLoginAsync(User User, CancellationToken cancellationToken)
        {
            var claims = new[]
            {
                new Claim(this.stringConfig.NameClaimsJWTUser, User.Id_User.ToString(), ClaimValueTypes.String),
            };

            SessionUserDto JWTGenerate = this.GenerateJwtToken(claims, DateTime.UtcNow.AddDays(7));

            User.token_session_user = JWTGenerate.token_session_user;

            User.date_token_session_expiration = JWTGenerate.date_token_session_expiration;

            await this._repositoryU.SaveChangesAsync(cancellationToken);

            this.SetSessionCookie(this._httpContextAccessor.HttpContext.Response, this.stringConfig.NameCookieUserConnected, JWTGenerate);
        }


        public void SetCookieToSaveIdGoogle(HttpResponse Response, string userIdGoogle, string EmailGoogle)
        {
            DateTime expirationCookieGoogle = DateTime.UtcNow.AddHours(1);

            var claims = new[]
            {
                new Claim(this.stringConfig.NameUserIdGoogle, userIdGoogle),
                new Claim(this.stringConfig.NameUserEmailGoogle, EmailGoogle)
            };

            SessionUserDto sessionDataJWT = this.GenerateJwtToken(claims, expirationCookieGoogle);

            this.SetSessionCookie(Response, this.stringConfig.NameCookieUserGoogle, sessionDataJWT);
        }
    }



   public interface IJWTSessionUtils
   {
        SessionUserDto GenerateJwtToken(Claim[] claims, DateTime expirationValue);
        void SetSessionCookie(HttpResponse Response, string nameOfCookie, SessionUserDto sessionData);
        IEnumerable<Claim> GetClaimsFromCookieJWT(HttpContext httpContext, string nameOfCookie);
        string GetValueClaimsCookieUser(HttpContext httpContext);
        string GetCookie(HttpContext httpContext, string nameOfCookie);
        Task CreateSessionLoginAsync(User User, CancellationToken cancellationToken);
        void SetCookieToSaveIdGoogle(HttpResponse Response, string userIdGoogle, string EmailGoogle);
    }
}