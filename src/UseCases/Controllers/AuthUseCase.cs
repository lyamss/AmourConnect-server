using Application.Interfaces.Controllers;
using Domain.Dtos.AppLayerDtos;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Domain.Dtos.SetDtos;
using Application.Interfaces.Services;
using Application.Interfaces.Services.Email;
using Application.Services;
using Domain.Utils;
using Microsoft.Extensions.Options;
namespace Application.UseCases.Controllers
{
    internal sealed class AuthUseCase(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, IRegexUtils regexUtils, ISendMail sendMail, IJWTSessionUtils jWTSessionUtils, IOptions<SecretEnv> SecretEnv) : IAuthUseCase
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IRegexUtils _regexUtils = regexUtils;
        private readonly ISendMail sendMail = sendMail;
        private readonly IJWTSessionUtils _jWTSessions = jWTSessionUtils;

        public async Task ValidateGoogleLoginAsync()
        {
            var response = await _httpContextAccessor.HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (response?.Principal == null || !response.Succeeded)
            {
                throw new ExceptionAPI(false, SecretEnv.Value.Ip_Now_Frontend + "/login", null);
            }

            var EmailGoogle = response.Principal.FindFirstValue(ClaimTypes.Email);
            var userIdGoogle = response.Principal.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(EmailGoogle) || string.IsNullOrEmpty(userIdGoogle))
            {
                throw new ExceptionAPI(false, SecretEnv.Value.Ip_Now_Frontend + "/login", null);
            }

            int? Id_User = await _userRepository.GetUserIdWithGoogleIdAsync(EmailGoogle, userIdGoogle);

            if (Id_User > 0)
            {
                await CreateSessionLoginAsync(Id_User.Value);
                throw new ExceptionAPI(true, SecretEnv.Value.Ip_Now_Frontend + "/welcome", null);
            }

            SetCookieToSaveIdGoogle(_httpContextAccessor.HttpContext.Response, userIdGoogle, EmailGoogle);
            throw new ExceptionAPI(false, SecretEnv.Value.Ip_Now_Frontend + "/register", null);
        }


        public async Task RegisterUserAsync(SetUserRegistrationDto setuserRegistrationDto)
        {
            var claims = _jWTSessions.GetClaimsFromCookieJWT(_httpContextAccessor.HttpContext, _jWTSessions.NameCookieUserGoogle);

            string emailGoogle = claims?.FirstOrDefault(c => c.Type == "EmailGoogle")?.Value;
            string userIdGoogle = claims?.FirstOrDefault(c => c.Type == "userIdGoogle")?.Value;

            if (string.IsNullOrEmpty(emailGoogle) || string.IsNullOrEmpty(userIdGoogle))
            {
                throw new ExceptionAPI(false, "Please login with Google before register", null);
            }

            var (success, message) = _regexUtils.CheckBodyAuthRegister(setuserRegistrationDto);

            if (!success)
            {
                throw new ExceptionAPI(success, message, null);
            }

            if (await _userRepository.GetUserByPseudoAsync(setuserRegistrationDto.Pseudo))
            {
                throw new ExceptionAPI(false, "Pseudo Already use", null);
            }

            int? id_user = await _userRepository.GetUserIdWithGoogleIdAsync(emailGoogle, userIdGoogle);

            if (id_user > 0)
            {
                await CreateSessionLoginAsync(id_user.Value);
                throw new ExceptionAPI(true, "User already exists, logged in", null);
            }

            int? Newid_user = await _userRepository.CreateUserAsync(userIdGoogle, emailGoogle, setuserRegistrationDto);

            if (Newid_user.HasValue)
            {
                await CreateSessionLoginAsync(Newid_user.Value);
                await sendMail.MailRegisterAsync(emailGoogle, setuserRegistrationDto.Pseudo);
                throw new ExceptionAPI(true, "Register finish", null);
            }

            throw new ExceptionAPI(false, "Failed to create user", null);
        }

        private async Task CreateSessionLoginAsync(int Id_User)
        {
            var claims = new[]
{
                new Claim("userAmourConnected", Id_User.ToString(), ClaimValueTypes.String),
            };
            SessionUserDto JWTGenerate = _jWTSessions.GenerateJwtToken(claims, DateTime.UtcNow.AddDays(7));
            await _userRepository.UpdateSessionUserAsync(Id_User, JWTGenerate);
            _jWTSessions.SetSessionCookie(_httpContextAccessor.HttpContext.Response, _jWTSessions.NameCookieUserConnected, JWTGenerate);
        }

        private void SetCookieToSaveIdGoogle(HttpResponse Response, string userIdGoogle, string EmailGoogle)
        {
            DateTime expirationCookieGoogle = DateTime.UtcNow.AddHours(1);

            var claims = new[]
            {
                new Claim("userIdGoogle", userIdGoogle),
                new Claim("EmailGoogle", EmailGoogle)
            };

            SessionUserDto sessionDataJWT = _jWTSessions.GenerateJwtToken(claims, expirationCookieGoogle);

            _jWTSessions.SetSessionCookie(Response, _jWTSessions.NameCookieUserGoogle, sessionDataJWT);
        }
    }
}