using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc;
using API.Features.Authentification.Filters;
using API.Features.Users;
using API.Entities;
using API.Features.RequestFriend;



namespace API.Features.Message;


[ServiceFilter(typeof(AuthorizeAuth))]
public class ChatHub
(
        IUserRepository userRepository, IRequestFriendsRepository RequestFriendsRepository, IMessageRepository MessageRepository, DataUser _dataUser
)
: Hub
{

    private readonly IUserRepository _userRepository = userRepository;
    private readonly IRequestFriendsRepository _requestFriendsRepository = RequestFriendsRepository;
    private readonly IMessageRepository _messageRepository = MessageRepository;
    private readonly DataUser dataUser = _dataUser;
    

    public async Task GetUserMessage(int Id_UserReceiver, CancellationToken cancellationToken)
    {
        User dataUserNowConnect = await this.dataUser.GetDataUserConnected(cancellationToken);

        RequestFriendForGetMessageDto existingRequest = await this._requestFriendsRepository.GetRequestFriendByIdAsync(dataUserNowConnect.Id_User, Id_UserReceiver, cancellationToken);

        if (existingRequest is null)
        {
            await this.Clients.Caller.SendAsync("ReceiveError", "You have to match to talk together");
            return;
        }

        if (existingRequest.Status == RequestStatus.Onhold)
        {
            await this.Clients.Caller.SendAsync("ReceiveError", "There must be validation of the match request to chat");
            return;
        }

        ICollection<QueryMessage> msg = await this._messageRepository.GetMessagesAsync(dataUserNowConnect.Id_User, Id_UserReceiver, cancellationToken);

        await this.Clients.User(Id_UserReceiver.ToString()).SendAsync("ReceiveMessage", msg, cancellationToken);
        await this.Clients.Caller.SendAsync("ReceiveMessage", msg, cancellationToken);
        return;
    }
}