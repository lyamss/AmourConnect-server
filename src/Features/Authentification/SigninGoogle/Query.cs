using API.Services;
using MediatR;

namespace API.Features.Authentification.SigninGoogle;

public record Query() : IRequest<ApiResponseDto>;