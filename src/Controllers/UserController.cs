using Domain.Dtos.GetDtos;
using Domain.Dtos.SetDtos;
using Microsoft.AspNetCore.Mvc;
using API.Filters;
using Application.Interfaces.Controllers;
using Application.Services;
using Domain.Dtos.AppLayerDtos;
using Domain.Entities;
namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(AuthorizeAuth))]
    public class UserController(IUserUseCase userUseCase) : ControllerBase
    {
        private readonly IUserUseCase _userUseCase = userUseCase;


        [HttpGet("GetUsersToMach")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<GetUserDto>))]
        public async Task<IActionResult> GetUsersToMach()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ApiResponseDto<IEnumerable<GetUserDto>> _responseApi = null;

            try { await _userUseCase.GetUsersToMach(); }
            
            catch (ExceptionAPI e) { var objt = e.ManageApiMessage<IEnumerable<GetUserDto>>(); _responseApi = objt; }

            return Ok(_responseApi);
        }



        [HttpGet("GetUserConnected")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<GetUserDto>))]
        public async Task<IActionResult> GetUserConnected()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ApiResponseDto<GetUserDto> _responseApi = null;

            try { await _userUseCase.GetUserConnected(); }

            catch (ExceptionAPI e) { var objt = e.ManageApiMessage<GetUserDto>(); _responseApi = objt; }

            return Ok(_responseApi);
        }


        [HttpPatch("UpdateUser")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<GetUserDto>))]
        public async Task<IActionResult> UpdateUser([FromForm] SetUserUpdateDto setUserUpdateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ApiResponseDto<GetUserDto> _responseApi = null; 

            try { await _userUseCase.UpdateUser(setUserUpdateDto); }

            catch (ExceptionAPI e) { var objt = e.ManageApiMessage<GetUserDto>(); _responseApi = objt; }
            
            return Ok(_responseApi);
        }


        [HttpGet("GetUser/{Id_User}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<GetUserDto>))]
        public async Task<IActionResult> GetUserId([FromRoute] int Id_User)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ApiResponseDto<GetUserDto> _responseApi = null;

            try { await _userUseCase.GetUserById(Id_User); }

            catch(ExceptionAPI e) { var objt = e.ManageApiMessage<GetUserDto>(); _responseApi = objt; }

            return _responseApi.Message == "no found :/"
            ? NotFound()
            : Ok(_responseApi);
        }
    }
}