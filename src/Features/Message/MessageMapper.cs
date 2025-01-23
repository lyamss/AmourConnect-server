namespace API.Features.Message
{
    public static class MessageMapper
    {
        public static QueryMessage ToGetMessageMapper(this  Entities.Message message)
        {
            if (message == null)
            {
                return null;
            }

            return new QueryMessage
            (
                message.Id_Message,
                message.message_content,
                message.Date_of_request,
                message.IdUserIssuer,
                message.Id_UserReceiver,
                message.UserReceiver.Pseudo,            
                message.UserIssuer.Pseudo
            );
        }
    }
}