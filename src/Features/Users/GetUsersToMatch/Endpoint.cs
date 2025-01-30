using API.Features.Users.GetUsersToMach;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Users;
public partial class UserController
{
    [HttpGet("GetUsersToMach")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<QueryUser>))]
    public async Task<IActionResult> GetUsersToMach(CancellationToken cancellationToken)
    {
        var q = new Query();
        ApiResponseDto apiResponseDto = await this.mediator.Send(q, cancellationToken);

        return apiResponseDto.SuccesResponse ? this.Ok(apiResponseDto) : this.BadRequest(apiResponseDto);
    }
}