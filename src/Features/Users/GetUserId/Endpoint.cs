using API.Features.Users.GetUserId;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Users;

public partial class UserController
{

    [HttpGet("GetUser/{Id_User}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<QueryUser>))]
    public async Task<IActionResult> GetUserId([FromRoute] int Id_User, CancellationToken cancellationToken)
    {
        if (!this.ModelState.IsValid)
            return this.BadRequest(this.ModelState);

        CommandGetUserId commandGetUserId = new(Id_User);

        ApiResponseDto apiResponseDto = await this.mediator.Send(commandGetUserId, cancellationToken);

        return apiResponseDto.SuccesResponse ? this.Ok(apiResponseDto) : this.NotFound(apiResponseDto);
    }

}