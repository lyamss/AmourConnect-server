using Domain.Dtos.GetDtos;
using Domain.Dtos.SetDtos;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using API.Filters;
using Application.Interfaces.Controllers;
using Domain.Dtos.AppLayerDtos;
namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(AuthorizeAuth))]
    public class MessageController(IMessageUseCase MessageUseCase) : ControllerBase
    {
        private readonly IMessageUseCase _messageUseCase = MessageUseCase;

        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] SetMessageDto setmessageDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ApiResponseDto<string> _responseApi = null;

            try { await _messageUseCase.SendMessageAsync(setmessageDto); }

            catch (ExceptionAPI e) { var objt = e.ManageApiMessage<string>(); _responseApi = objt; }

            return _responseApi.Message == "There must be validation of the friend request to chat"
            ? Conflict(_responseApi)
            : _responseApi.Success
                ? Ok(_responseApi)
                : BadRequest(_responseApi);
        }



        [HttpGet("GetUserMessage/{Id_UserReceiver}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<GetMessageDto>))]
        public async Task<IActionResult> GetUserMessage([FromRoute] int Id_UserReceiver)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ApiResponseDto<ICollection<GetMessageDto>> _responseApi = null;

            try { await _messageUseCase.GetUserMessagesAsync(Id_UserReceiver); }

            catch (ExceptionAPI e) { var objt = e.ManageApiMessage<ICollection<GetMessageDto>>(); _responseApi = objt; }

            return _responseApi.Success
            ? Ok(_responseApi) 
            : Conflict(_responseApi);
        }
    }
}