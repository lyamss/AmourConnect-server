using API.Entities;
using API.Services;
using MediatR;

namespace API.Features.Users.GetUsersToMach;
internal sealed class Handler : IRequestHandler<Query, ApiResponseDto>
{
    private readonly DataUser dataUser;
    private readonly IUserRepository userRepository;

    public Handler(DataUser _dataUser, IUserRepository _userRepository)
    {
        this.dataUser = _dataUser;
        this.userRepository = _userRepository;
    }

    public async Task<ApiResponseDto> Handle(Query query, CancellationToken cancellationToken)
    {
        User dataUserNowConnect = await this.dataUser.GetDataUserConnected(cancellationToken);

        ICollection<QueryUser> users = await this.userRepository.GetUsersToMatchAsync(dataUserNowConnect, cancellationToken);

        return ApiResponseDto.Success("yes good", users);
    }
}