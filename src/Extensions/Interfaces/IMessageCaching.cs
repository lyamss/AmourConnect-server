using Domain.Dtos.GetDtos;

namespace Infrastructure.Interfaces
{
    public interface IMessageCaching
    {
        Task<ICollection<GetMessageDto>> GetMessagesAsync(int idUserIssuer, int idUserReceiver);
    }
}