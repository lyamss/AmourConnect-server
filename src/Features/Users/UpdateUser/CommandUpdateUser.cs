using API.Services;
using MediatR;

namespace API.Features.Users.UpdateUser
{
    public record CommandUpdateUser(IFormFile? Profile_picture, string? city, string? Description, string? sex, DateTime? date_of_birth) : IRequest<ApiResponseDto>;
}