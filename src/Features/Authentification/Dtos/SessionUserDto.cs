namespace API.Features.Authentification.Dtos
{
    public record SessionUserDto(string token_session_user, DateTime date_token_session_expiration);
}