namespace API.Features.Users
{
    public record CommandUpdateUser(IFormFile? Profile_picture, string? city, string? Description, string? sex, DateTime? date_of_birth);
}