using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using MediatR;
using API.Features.Users;
using API.Services;
using Microsoft.Extensions.Options;



namespace API.Features.Authentification.SigninGoogle
{
    internal sealed class Handler
    (IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, 
    IJWTSessionUtils jWTSessionUtils
    , IOptions<SecretEnv> SecretEnv) 
    : IRequestHandler<Query, ApiResponseDto>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IJWTSessionUtils _jWTSessions = jWTSessionUtils;

        public async Task<ApiResponseDto> Handle(Query query, CancellationToken cancellationToken)
        {
            var response = await this._httpContextAccessor.HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            
            if (response?.Principal == null || !response.Succeeded)
            {
                return ApiResponseDto.Failure( SecretEnv.Value.IpFrontend + "/login");
            }

            var EmailGoogle = response.Principal.FindFirstValue(ClaimTypes.Email);
            var userIdGoogle = response.Principal.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(EmailGoogle) || string.IsNullOrEmpty(userIdGoogle))
            {
                return ApiResponseDto.Failure(SecretEnv.Value.IpFrontend + "/login");
            }

            var Id_User = await this._userRepository.GetUserIdWithGoogleIdAsync(EmailGoogle, userIdGoogle, cancellationToken);

            if (Id_User is not null)
            {
                await this._jWTSessions.CreateSessionLoginAsync(Id_User, cancellationToken);
                return ApiResponseDto.Success(SecretEnv.Value.IpFrontend + "/welcome", null);
            }

            this._jWTSessions.SetCookieToSaveIdGoogle(this._httpContextAccessor.HttpContext.Response, userIdGoogle, EmailGoogle);
            return ApiResponseDto.Failure(SecretEnv.Value.IpFrontend + "/register");
        }
    }
}