using Application.Interfaces.Filters;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Application.Interfaces.Services;
using Domain.Entities;
namespace Application.UseCases.Filters
{
    internal sealed class AuthorizeAuthUseCase(IHttpContextAccessor httpContextAccessor, IJWTSessionUtils jWTSessionUtils, IUserCaching userCaching) : Attribute, IAuthorizeAuthUseCase, IAsyncAuthorizationFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IJWTSessionUtils _jWTSessions = jWTSessionUtils;
        private readonly IUserCaching _userCaching = userCaching;

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var cookieValueJWT = _jWTSessions.GetValueClaimsCookieUser(context.HttpContext);

            if (cookieValueJWT == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            string cookieValue = _jWTSessions.GetCookie(_httpContextAccessor.HttpContext, _jWTSessions.NameCookieUserConnected);

            User user = await _userCaching.GetUserWithCookieAsync(cookieValue);

            DateTime expirationDate = DateTime.UtcNow;
            if (user == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (user.date_token_session_expiration < expirationDate)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }
    }
}