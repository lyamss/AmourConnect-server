using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using API.Services;
using API.Entities;
using API.Features.Users;



namespace API.Features.Authentification.Filters
{

    internal sealed class AuthorizeAuthUseCase(IHttpContextAccessor httpContextAccessor, IJWTSessionUtils jWTSessionUtils, IUserRepository userRepository, StringConfig _stringConfig) : IAuthorizeAuthUseCase
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IJWTSessionUtils _jWTSessions = jWTSessionUtils;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly StringConfig stringConfig = _stringConfig;

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var cookieValueJWT = this._jWTSessions.GetValueClaimsCookieUser(context.HttpContext);

            if (cookieValueJWT == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            string cookieValue = this._jWTSessions.GetCookie(this._httpContextAccessor.HttpContext, this.stringConfig.NameCookieUserConnected);

            User user = await this._userRepository.GetUserWithCookieAsync(cookieValue, context.HttpContext.RequestAborted);

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