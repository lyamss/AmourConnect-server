using Microsoft.AspNetCore.Mvc;
using API.Features.Authentification.Filters;
using API.Services.Email;
using API.Services;
using API.Entities;
using MediatR;



namespace API.Features.RequestFriend
{
    [Route("api/v1/[controller]")]
    [ServiceFilter(typeof(AuthorizeAuth))]
    public partial class RequestFriendsController
    (
    IRepository<User> userRepository, IRequestFriendsRepository requestFriendsRepository, 
    ISendMail sendMail, IRepository<RequestFriends> repositoryR,
    IMediator _mediator, DataUser _dataUser
    ) : ControllerBase
    {


        private readonly IRepository<User> _userRepository = userRepository;
        private readonly IRequestFriendsRepository _requestFriendsRepository = requestFriendsRepository;
        private readonly ISendMail sendMail = sendMail;
        private readonly IRepository<RequestFriends> _repositoryR = repositoryR;
        private readonly IMediator mediator = _mediator;
        private readonly DataUser dataUser = _dataUser;


        [HttpGet("GetRequestFriends")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ICollection<QueryRequestFriend>>))]
        public async Task<IActionResult> GetRequestFriends(CancellationToken cancellationToken)
        {
            User dataUserNowConnect = await this.dataUser.GetDataUserConnected(cancellationToken);

            ICollection<QueryRequestFriend> requestFriends = await this._requestFriendsRepository.GetRequestFriendsAsync(dataUserNowConnect.Id_User, cancellationToken);

            List<QueryRequestFriend> filteredRequestFriends = new();

            foreach(QueryRequestFriend requestFriend in requestFriends)
            {
                requestFriend.UserReceiverPictureProfile = dataUserNowConnect.Id_User == requestFriend.Id_UserReceiver ? null : requestFriend.UserReceiverPictureProfile;
                requestFriend.UserIssuerPictureProfile = dataUserNowConnect.Id_User != requestFriend.Id_UserReceiver ? null : requestFriend.UserIssuerPictureProfile;

                filteredRequestFriends.Add(requestFriend);
            }

            requestFriends = filteredRequestFriends;

            return this.Ok(ApiResponseDto.Success("Request friends retrieved successfully", requestFriends));
        }



        [HttpPost("AddRequest/{IdUserReceiver}")]
        public async Task<IActionResult> RequestFriends([FromRoute] int IdUserReceiver, CancellationToken cancellationToken)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            User dataUserNowConnect = await this.dataUser.GetDataUserConnected(cancellationToken);

            User userReceiver = await this._userRepository.GetByIdAsync(IdUserReceiver, cancellationToken);

            if (userReceiver == null)
            {
                return this.BadRequest(ApiResponseDto.Failure("User receiver does not exist"));
            }

            if (dataUserNowConnect.Id_User == userReceiver.Id_User)
            {
                return this.Conflict(ApiResponseDto.Failure("User cannot send a match request to themselves"));
            }

            RequestFriendForGetMessageDto existingRequest = await this._requestFriendsRepository.GetRequestFriendByIdAsync(dataUserNowConnect.Id_User, IdUserReceiver, cancellationToken);

            if (existingRequest is null)
            {
                return this.Conflict(ApiResponseDto.Failure("You have already matched with this user"));
            }

            if (existingRequest.Status == RequestStatus.Onhold)
            {
                return this.Conflict(ApiResponseDto.Failure("A match request is already pending between these users"));
            }

            RequestFriends requestFriends = new()
            {
                UserIssuer = dataUserNowConnect,
                UserReceiver = userReceiver,
                Status = RequestStatus.Onhold,
                Date_of_request = DateTime.Now.ToUniversalTime()
            };

            await this._repositoryR.AddAsync(requestFriends, cancellationToken);
            await this._repositoryR.SaveChangesAsync(cancellationToken);

            await sendMail.RequestFriendMailAsync(userReceiver, dataUserNowConnect);

            return this.Ok(ApiResponseDto.Success("Your match request has been made successfully 💕", null));
        }
    }
}