using API.Services;
using MediatR;

namespace API.Features.Users.GetUsersToMach;
public record Query() : IRequest<ApiResponseDto>;