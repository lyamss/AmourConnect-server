using API.Features.Users.UpdateUser;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Users;

public partial class UserController
{
        [HttpPatch("UpdateUser")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<QueryUser>))]
        public async Task<IActionResult> UpdateUser([FromForm] CommandUpdateUser commandUpdateUser, CancellationToken cancellationToken)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            ApiResponseDto apiResponseDto = await this.mediator.Send(commandUpdateUser, cancellationToken);
            
            return apiResponseDto.SuccesResponse ? this.Ok(apiResponseDto) : this.BadRequest(apiResponseDto);
        }
}