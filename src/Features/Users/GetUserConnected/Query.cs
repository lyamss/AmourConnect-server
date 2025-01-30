using API.Services;
using MediatR;

namespace API.Features.Users.GetUserConnected;

public record Query() : IRequest<ApiResponseDto>;