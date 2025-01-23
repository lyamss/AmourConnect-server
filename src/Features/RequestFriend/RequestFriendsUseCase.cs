using Application.Interfaces.Controllers;
using Application.Interfaces.Services;
using Application.Interfaces.Services.Email;
using Application.Services;
using Domain.Dtos.AppLayerDtos;
using Domain.Dtos.GetDtos;
using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.Controllers
{
    internal sealed class RequestFriendsUseCase(IUserRepository userRepository, IRequestFriendsRepository requestFriendsRepository, IHttpContextAccessor httpContextAccessor, ISendMail sendMail, IJWTSessionUtils jWTSessionUtils, IRequestFriendsCaching requestFriendsCaching, IUserCaching userCaching) : IRequestFriendsUseCase
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IRequestFriendsRepository _requestFriendsRepository = requestFriendsRepository;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly string token_session_user = jWTSessionUtils.GetValueClaimsCookieUser(httpContextAccessor.HttpContext);
        private readonly ISendMail sendMail = sendMail;
        private readonly IRequestFriendsCaching _requestFriendsCaching = requestFriendsCaching;
        private readonly IUserCaching _userCaching = userCaching;

        public async Task GetRequestFriendsAsync()
        {
            User dataUserNowConnect = await _GetDataUserConnected(token_session_user);

            ICollection<GetRequestFriendsDto> requestFriends = await _requestFriendsCaching.GetRequestFriendsAsync(dataUserNowConnect.Id_User);

            List<GetRequestFriendsDto> filteredRequestFriends = new();

            foreach(GetRequestFriendsDto requestFriend in requestFriends)
            {
                requestFriend.UserReceiverPictureProfile = dataUserNowConnect.Id_User == requestFriend.Id_UserReceiver ? null : requestFriend.UserReceiverPictureProfile;
                requestFriend.UserIssuerPictureProfile = dataUserNowConnect.Id_User != requestFriend.Id_UserReceiver ? null : requestFriend.UserIssuerPictureProfile;

                filteredRequestFriends.Add(requestFriend);
            }

            requestFriends = filteredRequestFriends;

            throw new ExceptionAPI(true, "Request friends retrieved successfully", requestFriends);
        }

        public async Task AcceptFriendRequestAsync(int IdUserIssuer)
        {
            User dataUserNowConnect = await _GetDataUserConnectedWithDb(token_session_user);

            RequestFriends friendRequest = await _requestFriendsRepository.GetUserFriendRequestByIdAsync(dataUserNowConnect.Id_User, IdUserIssuer);

            if (friendRequest == null)
            {
                throw new ExceptionAPI(false, "Match request not found", null);
            }

            friendRequest.Status = RequestStatus.Accepted;

            await _requestFriendsRepository.UpdateStatusRequestFriendsAsync(friendRequest);

            await sendMail.AcceptRequestFriendMailAsync(friendRequest.UserIssuer, dataUserNowConnect);

            throw new ExceptionAPI(true, "Request match accepted", null);
        }

        public async Task AddRequestFriendsAsync(int IdUserReceiver)
        {
            User dataUserNowConnect = await _GetDataUserConnectedWithDb(token_session_user);

            User userReceiver = await _userRepository.GetUserByIdUserAsync(IdUserReceiver);

            if (userReceiver == null)
            {
                throw new ExceptionAPI(false, "User receiver does not exist", null);
            }

            if (dataUserNowConnect.Id_User == userReceiver.Id_User)
            {
                throw new ExceptionAPI(false, "User cannot send a match request to themselves", null);
            }

            RequestFriendForGetMessageDto existingRequest = await _requestFriendsRepository.GetRequestFriendByIdAsync(dataUserNowConnect.Id_User, IdUserReceiver);

            if (existingRequest != null)
            {
                if (existingRequest.Status == RequestStatus.Onhold)
                {
                    throw new ExceptionAPI(false, "A match request is already pending between these users", null);
                }

                throw new ExceptionAPI(false, "You have already matched with this user", null);
            }

            RequestFriends requestFriends = new()
            {
                UserIssuer = dataUserNowConnect,
                UserReceiver = userReceiver,
                Status = RequestStatus.Onhold,
                Date_of_request = DateTime.Now.ToUniversalTime()
            };

            await _requestFriendsRepository.AddRequestFriendAsync(requestFriends);

            await sendMail.RequestFriendMailAsync(userReceiver, dataUserNowConnect);

            throw new ExceptionAPI(true, "Your match request has been made successfully 💕", null);
        }

        private async Task<User> _GetDataUserConnected(string token_session_user) => await _userCaching.GetUserWithCookieAsync(token_session_user);
        private async Task<User> _GetDataUserConnectedWithDb(string token_session_user) => await _userRepository.GetUserWithCookieAsync(token_session_user);
    }
}