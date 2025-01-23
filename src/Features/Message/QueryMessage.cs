namespace API.Features.Message
{
    public record QueryMessage(int Id_Message, string message_content, DateTime Date_of_request, int IdUserIssuer, int Id_UserReceiver, string UserReceiverPseudo, string UserIssuerPseudo);
}