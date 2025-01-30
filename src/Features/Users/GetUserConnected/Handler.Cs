using API.Entities;
using API.Services;
using MediatR;

namespace API.Features.Users.GetUserConnected;

internal sealed class Handler : IRequestHandler<Query, ApiResponseDto>
{
    private readonly DataUser dataUser;

    public Handler(DataUser _dataUser)
    {
        this.dataUser = _dataUser;
    }

    public async Task<ApiResponseDto> Handle(Query query, CancellationToken cancellationToken)
    {
        User dataUserNowConnect = await this.dataUser.GetDataUserConnected(cancellationToken);

        QueryUser userDto = dataUserNowConnect.ToGetUserMapper();

        return ApiResponseDto.Success("yes good", userDto);
    }

}