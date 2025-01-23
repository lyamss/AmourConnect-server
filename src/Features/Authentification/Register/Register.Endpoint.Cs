using Microsoft.AspNetCore.Mvc;
using API.Features.Authentification.Register;
using API.Services;

namespace API.Features.Authentification;


public partial class Authentification
{
       [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CommandRegister commandRegister, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            ApiResponseDto _responseApi = await this.mediator.Send(commandRegister, cancellationToken);


            return (_responseApi.SuccesResponse)
            ? this.Ok(_responseApi)
            : this.BadRequest(_responseApi);
        }
}