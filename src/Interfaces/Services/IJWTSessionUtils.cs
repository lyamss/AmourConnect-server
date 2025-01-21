using Domain.Dtos.AppLayerDtos;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Interfaces.Services
{
   public interface IJWTSessionUtils
   {
        SessionUserDto GenerateJwtToken(Claim[] claims, DateTime expirationValue);
        void SetSessionCookie(HttpResponse Response, string nameOfCookie, SessionUserDto sessionData);
        public string NameCookieUserConnected { get; }
        public string NameCookieUserGoogle { get; }
        IEnumerable<Claim> GetClaimsFromCookieJWT(HttpContext httpContext, string nameOfCookie);

        string GetValueClaimsCookieUser(HttpContext httpContext);
        string GetCookie(HttpContext httpContext, string nameOfCookie);
    }
}