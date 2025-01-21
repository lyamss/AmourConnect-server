using Domain.Entities;
using Domain.Dtos.GetDtos;
namespace Domain.Mappers
{
    public static class MessageMapper
    {
        public static GetMessageDto ToGetMessageMapper(this Message message)
        {
            if (message == null)
            {
                return null;
            }

            return new GetMessageDto
            {
                Id_Message = message.Id_Message,
                message_content = message.message_content,
                IdUserIssuer = message.IdUserIssuer,
                UserIssuerPseudo = message.UserIssuer.Pseudo,
                Id_UserReceiver = message.Id_UserReceiver,
                UserReceiverPseudo = message.UserReceiver.Pseudo,
                Date_of_request = message.Date_of_request
            };
        }
    }
}