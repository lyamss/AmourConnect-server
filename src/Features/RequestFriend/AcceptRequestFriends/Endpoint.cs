using API.Features.RequestFriend.AcceptRequestFriends;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.RequestFriend;

public partial class RequestFriendsController
{
     [HttpPatch("AcceptRequestFriends/{IdUserIssuer}")]
    public async Task<IActionResult> AcceptFriendRequest([FromRoute] int IdUserIssuer, CancellationToken cancellationToken)
    {
        if (!this.ModelState.IsValid)
            return this.BadRequest(this.ModelState);

        Command command = new(IdUserIssuer);
        ApiResponseDto apiResponseDto = await this.mediator.Send(command, cancellationToken);

        return apiResponseDto.SuccesResponse ? this.Ok(apiResponseDto) : this.NotFound(apiResponseDto);
    }
}