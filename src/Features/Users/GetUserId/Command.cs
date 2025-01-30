using API.Services;
using MediatR;

namespace API.Features.Users.GetUserId;

public record CommandGetUserId(int Id_User) : IRequest<ApiResponseDto>;