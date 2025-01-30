using API.Services;
using MediatR;

namespace API.Features.RequestFriend.AcceptRequestFriends;

public record Command(int IdUserIssuer) : IRequest<ApiResponseDto>;