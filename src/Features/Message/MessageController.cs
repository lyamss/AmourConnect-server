using Microsoft.AspNetCore.Mvc;
using API.Features.Authentification.Filters;
using API.Features.Users;
using API.Services;
using API.Entities;
using API.Features.RequestFriend;
using Microsoft.AspNetCore.SignalR;



namespace API.Features.Message
{
    [Route("api/v1/[controller]")]
    [ServiceFilter(typeof(AuthorizeAuth))]
    public class MessageController
    (
        IUserRepository userRepository, IRequestFriendsRepository RequestFriendsRepository, IMessageRepository MessageRepository, 
        IRegexUtils regexUtils,
        IRepository<Entities.Message> repositoryM 
        ,IHubContext<ChatHub> _hubContext, DataUser _dataUser
    ) 
    : ControllerBase
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IRequestFriendsRepository _requestFriendsRepository = RequestFriendsRepository;
        private readonly IMessageRepository _messageRepository = MessageRepository;
        private readonly IRegexUtils _regexUtils = regexUtils;
        private readonly IRepository<Entities.Message> _repositoryM = repositoryM;
        private readonly IHubContext<ChatHub> hubContext = _hubContext;
        private readonly DataUser dataUser = _dataUser;

        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] CommandMessage commandMessage, CancellationToken cancellationToken)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

 
           User dataUserNowConnect = await this.dataUser.GetDataUserConnected(cancellationToken);

            RequestFriendForGetMessageDto existingRequest = await this._requestFriendsRepository.GetRequestFriendByIdAsync(dataUserNowConnect.Id_User, commandMessage.IdUserReceiver, cancellationToken);

            if (existingRequest is null)
            {
                return this.BadRequest(ApiResponseDto.Failure("You have to match to talk together"));
            }

            if (existingRequest.Status == RequestStatus.Onhold)
            {
                return this.Conflict(ApiResponseDto.Failure("There must be validation of the match request to chat"));
            }

            if (!_regexUtils.CheckMessage(commandMessage.MessageContent))
            {
                return this.BadRequest(ApiResponseDto.Failure("Message no valid"));
            }

            ICollection<QueryMessage> msg = await this._messageRepository.GetMessagesAsync(dataUserNowConnect.Id_User, commandMessage.IdUserReceiver, cancellationToken);

            var sortedMessages = msg.OrderBy(m => m.Date_of_request);

            if (sortedMessages.Count() > 50)
            {
                var messagesToDelete = sortedMessages.Take(30).Select(m => m.Id_Message).ToList();
                await this._messageRepository.DeleteMessagesAsync(messagesToDelete, cancellationToken);
            }

            var message = new Entities.Message
            {
                IdUserIssuer = dataUserNowConnect.Id_User,
                Id_UserReceiver = commandMessage.IdUserReceiver,
                message_content = commandMessage.MessageContent,
                Date_of_request = DateTime.Now.ToUniversalTime(),
            };

            await this._repositoryM.AddAsync(message, cancellationToken);
            await this._repositoryM.SaveChangesAsync(cancellationToken);

            await this.hubContext.Clients.User(commandMessage.IdUserReceiver.ToString()).SendAsync("ReceiveMessage", sortedMessages, cancellationToken);

            return this.Ok(ApiResponseDto.Success("Message send succes", null));
        }



        [HttpGet("GetUserMessage/{Id_UserReceiver}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<QueryMessage>))]
        public async Task<IActionResult> GetUserMessage([FromRoute] int Id_UserReceiver, CancellationToken cancellationToken)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);
                
           User dataUserNowConnect = await this.dataUser.GetDataUserConnected(cancellationToken);

            RequestFriendForGetMessageDto existingRequest = await this._requestFriendsRepository.GetRequestFriendByIdAsync(dataUserNowConnect.Id_User, Id_UserReceiver, cancellationToken);

            if (existingRequest is null)
            {
                return this.BadRequest(ApiResponseDto.Failure("You have to match to talk together"));
            }

            if (existingRequest.Status == RequestStatus.Onhold)
            {
                return this.BadRequest(ApiResponseDto.Failure("There must be validation of the match request to chat"));
            }

            ICollection<QueryMessage> msg = await this._messageRepository.GetMessagesAsync(dataUserNowConnect.Id_User, Id_UserReceiver, cancellationToken);

            await this.hubContext.Clients.User(Id_UserReceiver.ToString()).SendAsync("ReceiveMessage", msg, cancellationToken);

            return this.Ok(ApiResponseDto.Success("Messages retrieved successfully", msg));
        }
    }
}