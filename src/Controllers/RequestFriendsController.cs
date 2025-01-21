using Domain.Dtos.AppLayerDtos;
using Domain.Dtos.GetDtos;
using Microsoft.AspNetCore.Mvc;
using API.Filters;
using Application.Interfaces.Controllers;
using Application.Services;
namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(AuthorizeAuth))]
    public class RequestFriendsController(IRequestFriendsUseCase requestFriendsUseCase) : ControllerBase
    {
        private readonly IRequestFriendsUseCase _requestFriendsUseCase = requestFriendsUseCase;


        [HttpGet("GetRequestFriends")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ICollection<GetRequestFriendsDto>>))]
        public async Task<IActionResult> GetRequestFriends()
        {
            ApiResponseDto<ICollection<GetRequestFriendsDto>> _responseApi = null;

            try { await _requestFriendsUseCase.GetRequestFriendsAsync(); }

            catch (ExceptionAPI e) { var objt = e.ManageApiMessage<ICollection<GetRequestFriendsDto>>(); _responseApi = objt; }

            return _responseApi.Success
            ? Ok(_responseApi)
            : BadRequest(_responseApi);
        }



        [HttpPost("AddRequest/{IdUserReceiver}")]
        public async Task<IActionResult> RequestFriends([FromRoute] int IdUserReceiver)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ApiResponseDto<string> _responseApi = null;

            try { await _requestFriendsUseCase.AddRequestFriendsAsync(IdUserReceiver); }

            catch (ExceptionAPI e) { var objt = e.ManageApiMessage<string>(); _responseApi = objt; }

            return _responseApi.Message == "User receiver does not exist"
            ? BadRequest(_responseApi)
            : _responseApi.Success
                ? Ok(_responseApi)
                : Conflict(_responseApi);
        }


        [HttpPatch("AcceptRequestFriends/{IdUserIssuer}")]
        public async Task<IActionResult> AcceptFriendRequest([FromRoute] int IdUserIssuer)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ApiResponseDto<string> _responseApi = null;

            try { await _requestFriendsUseCase.AcceptFriendRequestAsync(IdUserIssuer); }

            catch (ExceptionAPI e) { var objt = e.ManageApiMessage<string>(); _responseApi = objt; }

            return _responseApi.Success
            ? Ok(_responseApi)
            : NotFound(_responseApi);
        }
    }
}