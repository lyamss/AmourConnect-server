using Domain.Dtos.AppLayerDtos;
using Domain.Dtos.SetDtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.Controllers;
using Microsoft.AspNetCore.Authentication.Google;
using Application.Services;
using Domain.Utils;
using Microsoft.Extensions.Options;
namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthUseCase authUseCase, IOptions<SecretEnv> SecretEnv) : ControllerBase
    {
        private readonly IAuthUseCase _authUseCase = authUseCase;

        [HttpGet("login")]
        public IActionResult Login() => Challenge(new AuthenticationProperties { RedirectUri = SecretEnv.Value.Ip_Now_Backend + "/api/Auth/signin-google" }, GoogleDefaults.AuthenticationScheme);



        [HttpGet("signin-google")]
        public async Task<IActionResult> GoogleLogin()
        {
            ApiResponseDto<string> _responseApi = null;

            try { await _authUseCase.ValidateGoogleLoginAsync(); }

            catch (ExceptionAPI e) { var objt = e.ManageApiMessage<string>(); _responseApi = objt; }

            return Redirect(_responseApi.Message);
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] SetUserRegistrationDto setuserRegistrationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ApiResponseDto<string> _responseApi = null;

            try { await _authUseCase.RegisterUserAsync(setuserRegistrationDto); }

            catch (ExceptionAPI e) { var objt = e.ManageApiMessage<string>(); _responseApi = objt; }

            return (_responseApi.Success)
            ? Ok(_responseApi)
            : BadRequest(_responseApi);
        }
    }
}