using Microsoft.AspNetCore.Mvc;
using API.Features.Authentification.Filters;
using API.Features.Users;
using API.Services;
using API.Entities;



namespace API.Features.Message
{
    [Route("api/v1/[controller]")]
    [ServiceFilter(typeof(AuthorizeAuth))]
    public class MessageController
    (
        IUserRepository userRepository, IRequestFriendsRepository RequestFriendsRepository, IMessageRepository MessageRepository, 
        IHttpContextAccessor httpContextAccessor, IRegexUtils regexUtils, IJWTSessionUtils jWTSessionUtils,
        IMessageRepository messageRepository,
        IRepository<Entities.Message> repositoryM
    ) 
    : ControllerBase
    {

        private readonly IUserRepository _userRepository = userRepository;
        private readonly IRequestFriendsRepository _requestFriendsRepository = RequestFriendsRepository;
        private readonly IMessageRepository _messageRepository = MessageRepository;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly string token_session_user = jWTSessionUtils.GetValueClaimsCookieUser(httpContextAccessor.HttpContext);
        private readonly IRegexUtils _regexUtils = regexUtils;
        private readonly IRepository<Entities.Message> _repositoryM = repositoryM;
        private async Task<User> _GetDataUserConnected(string token_session_user, CancellationToken cancellationToken) => await _userRepository.GetUserWithCookieAsync(token_session_user, cancellationToken);

        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] CommandMessage commandMessage, CancellationToken cancellationToken)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

 
           User dataUserNowConnect = await this._GetDataUserConnected(token_session_user, cancellationToken);

            RequestFriendForGetMessageDto existingRequest = await _requestFriendsRepository.GetRequestFriendByIdAsync(dataUserNowConnect.Id_User, setmessageDto.IdUserReceiver);

            if (existingRequest != null)
            {
                if (existingRequest.Status == RequestStatus.Onhold)
                {
                    return this.Conflict(ApiResponseDto.Failure("There must be validation of the match request to chat"));
                }

                if (!_regexUtils.CheckMessage(setmessageDto.MessageContent))
                {
                    return this.BadRequest(ApiResponseDto.Failure("Message no valid"));
                }

                var message = new Entities.Message
                {
                    IdUserIssuer = dataUserNowConnect.Id_User,
                    Id_UserReceiver = setmessageDto.IdUserReceiver,
                    message_content = setmessageDto.MessageContent,
                    Date_of_request = DateTime.Now.ToUniversalTime(),
                };

                await this._repositoryM.AddAsync(message, cancellationToken);
                await this._repositoryM.SaveChangesAsync(cancellationToken);

                return this.Ok(ApiResponseDto.Success("Message send succes", null));
            }
            return this.BadRequest(ApiResponseDto.Failure("You have to match to talk together"));
        }



        [HttpGet("GetUserMessage/{Id_UserReceiver}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<QueryMessage>))]
        public async Task<IActionResult> GetUserMessage([FromRoute] int Id_UserReceiver, CancellationToken cancellationToken)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            User dataUserNowConnect = await this._GetDataUserConnected(token_session_user, cancellationToken);

            RequestFriendForGetMessageDto existingRequest = await _requestFriendsCaching.GetRequestFriendByIdAsync(dataUserNowConnect.Id_User, Id_UserReceiver);

            if (existingRequest != null)
            {
                if (existingRequest.Status == RequestStatus.Onhold)
                {
                    return this.BadRequest(ApiResponseDto.Failure("There must be validation of the match request to chat"));
                }

                ICollection<QueryMessage> msg = await this._messageRepository.GetMessagesAsync(dataUserNowConnect.Id_User, Id_UserReceiver, cancellationToken);

                var sortedMessages = msg.OrderBy(m => m.Date_of_request);

                if (sortedMessages.Count() > 50)
                {
                    var messagesToDelete = sortedMessages.Take(30).Select(m => m.Id_Message).ToList();
                    await _messageRepository.DeleteMessagesAsync(messagesToDelete, cancellationToken);
                }

                return this.Ok(ApiResponseDto.Success("Messages retrieved successfully", msg));
            }
            return this.BadRequest(ApiResponseDto.Failure("You have to match to talk together"));
        }
    }
}