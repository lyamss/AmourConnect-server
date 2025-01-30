using API.Entities;
using API.Services;
using API.Services.Email;
using MediatR;

namespace API.Features.RequestFriend.AcceptRequestFriends;

internal sealed class Handler : IRequestHandler<Command, ApiResponseDto>
{
    private readonly DataUser dataUser;
    private readonly IRequestFriendsRepository requestFriendsRepository;
    private readonly ISendMail sendMail;
    private readonly IRepository<RequestFriends> repositoryR;

    public Handler(DataUser _dataUser, IRequestFriendsRepository _requestFriendsRepository, ISendMail _sendMail, IRepository<RequestFriends> _repositoryR)
    {
        this.dataUser = _dataUser;
        this.requestFriendsRepository = _requestFriendsRepository;
        this.sendMail = _sendMail;
        this.repositoryR = _repositoryR;
    }
    public async Task<ApiResponseDto> Handle(Command command, CancellationToken cancellationToken)
    {

        User dataUserNowConnect = await this.dataUser.GetDataUserConnected(cancellationToken);

        RequestFriends friendRequest = await this.requestFriendsRepository.GetUserFriendRequestByIdAsync(dataUserNowConnect.Id_User, command.IdUserIssuer, cancellationToken);

        if (friendRequest == null)
        {
            return  ApiResponseDto.Failure("Match request not found");
        }

        friendRequest.Status = RequestStatus.Accepted;

        await repositoryR.SaveChangesAsync(cancellationToken);

        await sendMail.AcceptRequestFriendMailAsync(friendRequest.UserIssuer, dataUserNowConnect);

        return ApiResponseDto.Success("Request match accepted", null);

    }

}