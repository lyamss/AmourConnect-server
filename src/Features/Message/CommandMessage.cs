namespace API.Features.Message
{
    public record CommandMessage(int IdUserReceiver, string MessageContent);
}