namespace API.Features.Users
{
    public record QueryUser(int Id_User, string Pseudo, string Description, byte[] Profile_picture, string City, string Sex, DateTime Date_of_birth);
}