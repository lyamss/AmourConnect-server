using Microsoft.AspNetCore.Mvc;
using API.Features.Authentification.Filters;
using API.Features.Users;
using API.Services.Email;
using API.Services;
using API.Entities;



namespace API.Features.RequestFriend
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(AuthorizeAuth))]
    public class RequestFriendsController
    (
    IUserRepository userRepository, IRequestFriendsRepository requestFriendsRepository, IHttpContextAccessor httpContextAccessor, 
    ISendMail sendMail, IJWTSessionUtils jWTSessionUtils
    ) : ControllerBase
    {


        private readonly IUserRepository _userRepository = userRepository;
        private readonly IRequestFriendsRepository _requestFriendsRepository = requestFriendsRepository;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly string token_session_user = jWTSessionUtils.GetValueClaimsCookieUser(httpContextAccessor.HttpContext);
        private readonly ISendMail sendMail = sendMail;
        private async Task<User> _GetDataUserConnected(string token_session_user, CancellationToken cancellationToken) => await this._userRepository.GetUserWithCookieAsync(token_session_user, cancellationToken);



        [HttpGet("GetRequestFriends")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ICollection<QueryRequestFriend>>))]
        public async Task<IActionResult> GetRequestFriends(CancellationToken cancellationToken)
        {
            User dataUserNowConnect = await this._GetDataUserConnected(token_session_user, cancellationToken);

            ICollection<QueryRequestFriend> requestFriends = await this._requestFriendsRepository.GetRequestFriendsAsync(dataUserNowConnect.Id_User, cancellationToken);

            List<QueryRequestFriend> filteredRequestFriends = new();

            foreach(QueryRequestFriend requestFriend in requestFriends)
            {
                requestFriend.UserReceiverPictureProfile = dataUserNowConnect.Id_User == requestFriend.Id_UserReceiver ? null : requestFriend.UserReceiverPictureProfile;
                requestFriend.UserIssuerPictureProfile = dataUserNowConnect.Id_User != requestFriend.Id_UserReceiver ? null : requestFriend.UserIssuerPictureProfile;

                filteredRequestFriends.Add(requestFriend);
            }

            requestFriends = filteredRequestFriends;

            throw new ExceptionAPI(true, "Request friends retrieved successfully", requestFriends);
        }



        [HttpPost("AddRequest/{IdUserReceiver}")]
        public async Task<IActionResult> RequestFriends([FromRoute] int IdUserReceiver)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ApiResponseDto<string> _responseApi = null;

            try { await _requestFriendsUseCase.AddRequestFriendsAsync(IdUserReceiver); }

            catch (ExceptionAPI e) { var objt = e.ManageApiMessage<string>(); _responseApi = objt; }

            return _responseApi.Message == "User receiver does not exist"
            ? BadRequest(_responseApi)
            : _responseApi.Success
                ? Ok(_responseApi)
                : Conflict(_responseApi);
        }


        [HttpPatch("AcceptRequestFriends/{IdUserIssuer}")]
        public async Task<IActionResult> AcceptFriendRequest([FromRoute] int IdUserIssuer)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ApiResponseDto<string> _responseApi = null;

            try { await _requestFriendsUseCase.AcceptFriendRequestAsync(IdUserIssuer); }

            catch (ExceptionAPI e) { var objt = e.ManageApiMessage<string>(); _responseApi = objt; }

            return _responseApi.Success
            ? Ok(_responseApi)
            : NotFound(_responseApi);
        }
    }
}