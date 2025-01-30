using API.Features.Users.GetUserConnected;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Users;

public partial class UserController
{

    [HttpGet("GetUserConnected")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<QueryUser>))]
    public async Task<IActionResult> GetUserConnected(CancellationToken cancellationToken)
    {   
        var q = new Query();
        ApiResponseDto apiResponseDto = await this.mediator.Send(q, cancellationToken);

        return apiResponseDto.SuccesResponse ? this.Ok(apiResponseDto) : this.BadRequest(apiResponseDto);
    }    

}