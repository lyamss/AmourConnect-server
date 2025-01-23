using API.Features.Authentification.SigninGoogle;
using API.Services;
using Microsoft.AspNetCore.Mvc;


namespace API.Features.Authentification;


public partial class Authentification
{
        [HttpGet("signin-google")]
        public async Task<IActionResult> GoogleLogin(CancellationToken cancellationToken)
        {

            Query query = new();
            ApiResponseDto _responseApi = await this.mediator.Send(query, cancellationToken);

            return this.Redirect(_responseApi.Message.ToString());
        }
}