using Application.Interfaces.Controllers;
using Application.Interfaces.Services;
using Application.Services;
using Domain.Dtos.AppLayerDtos;
using Domain.Dtos.GetDtos;
using Domain.Dtos.SetDtos;
using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.Controllers
{
    internal sealed class MessageUseCase(IUserRepository userRepository, IRequestFriendsRepository RequestFriendsRepository, IMessageRepository MessageRepository, IHttpContextAccessor httpContextAccessor, IRegexUtils regexUtils, IJWTSessionUtils jWTSessionUtils, IRequestFriendsCaching requestFriendsCaching, IUserCaching userCaching, IMessageCaching messageCaching) : IMessageUseCase
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IRequestFriendsRepository _requestFriendsRepository = RequestFriendsRepository;
        private readonly IMessageRepository _messageRepository = MessageRepository;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly string token_session_user = jWTSessionUtils.GetValueClaimsCookieUser(httpContextAccessor.HttpContext);
        private readonly IRegexUtils _regexUtils = regexUtils;
        private readonly IRequestFriendsCaching _requestFriendsCaching = requestFriendsCaching;
        private readonly IUserCaching _userCaching = userCaching;
        private readonly IMessageCaching _messageCaching = messageCaching;

        public async Task SendMessageAsync(SetMessageDto setmessageDto)
        {
            User dataUserNowConnect = await _GetDataUserConnected(token_session_user);

            RequestFriendForGetMessageDto existingRequest = await _requestFriendsRepository.GetRequestFriendByIdAsync(dataUserNowConnect.Id_User, setmessageDto.IdUserReceiver);

            if (existingRequest != null)
            {
                if (existingRequest.Status == RequestStatus.Onhold)
                {
                    throw new ExceptionAPI(false, "There must be validation of the match request to chat", null);
                }

                if (!_regexUtils.CheckMessage(setmessageDto.MessageContent))
                {
                    throw new ExceptionAPI(false, "Message no valid", null);
                }

                var message = new Message
                {
                    IdUserIssuer = dataUserNowConnect.Id_User,
                    Id_UserReceiver = setmessageDto.IdUserReceiver,
                    message_content = setmessageDto.MessageContent,
                    Date_of_request = DateTime.Now.ToUniversalTime(),
                };

                await _messageRepository.AddMessageAsync(message);

                throw new ExceptionAPI(true, "Message send succes", null);
            }
            throw new ExceptionAPI(false, "You have to match to talk together", null);
        }

        public async Task GetUserMessagesAsync(int Id_UserReceiver)
        {
            User dataUserNowConnect = await _GetDataUserConnectedWithCache(token_session_user);

            RequestFriendForGetMessageDto existingRequest = await _requestFriendsCaching.GetRequestFriendByIdAsync(dataUserNowConnect.Id_User, Id_UserReceiver);

            if (existingRequest != null)
            {
                if (existingRequest.Status == RequestStatus.Onhold)
                {
                    throw new ExceptionAPI(false, "There must be validation of the match request to chat", null);
                }

                ICollection<GetMessageDto> msg = await _messageCaching.GetMessagesAsync(dataUserNowConnect.Id_User, Id_UserReceiver);

                var sortedMessages = msg.OrderBy(m => m.Date_of_request);

                if (sortedMessages.Count() > 50)
                {
                    var messagesToDelete = sortedMessages.Take(30).Select(m => m.Id_Message).ToList();
                    await _messageRepository.DeleteMessagesAsync(messagesToDelete);
                }

                throw new ExceptionAPI(true, "Messages retrieved successfully", msg);
            }
            throw new ExceptionAPI(false, "You have to match to talk together", null);
        }
        private async Task<User> _GetDataUserConnected(string token_session_user) => await _userRepository.GetUserWithCookieAsync(token_session_user);
        private async Task<User> _GetDataUserConnectedWithCache(string token_session_user) => await _userCaching.GetUserWithCookieAsync(token_session_user);
    }
}