using Domain.Dtos.SetDtos;

namespace Application.Interfaces.Controllers
{
    public interface IMessageUseCase
    {
        Task SendMessageAsync(SetMessageDto setmessageDto);
        Task GetUserMessagesAsync(int Id_UserReceiver);
    }
}