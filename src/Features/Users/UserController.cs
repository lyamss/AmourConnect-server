using API.Entities;
using API.Features.Authentification.Filters;
using API.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace API.Features.Users
{
    [Route("api/v1/[controller]")]
    [ServiceFilter(typeof(AuthorizeAuth))]
    public class UserController
    (
    IMediator mediator,
    IUserRepository userRepository,
    IHttpContextAccessor httpContextAccessor,
    IJWTSessionUtils jWTSessionUtils,
    IRegexUtils regexUtils,
    IMessUtils messUtils,
    IRepository<User> repositoryU
    ) 
    : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly string token_session_user = jWTSessionUtils.GetValueClaimsCookieUser(httpContextAccessor.HttpContext);
        private readonly IRegexUtils _regexUtils = regexUtils;
        private readonly IMessUtils _messUtils = messUtils;
        private readonly IRepository<User> _repositoryU = repositoryU; 
        private async Task<User> _GetDataUserConnected(string token_session_user, CancellationToken cancellationToken) => await this._userRepository.GetUserWithCookieAsync(token_session_user, cancellationToken);


        [HttpGet("GetUsersToMach")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<QueryUser>))]
        public async Task<IActionResult> GetUsersToMach(CancellationToken cancellationToken)
        {
            User dataUserNowConnect = await this._GetDataUserConnected(token_session_user, cancellationToken);

            ICollection<QueryUser> users = await this._userRepository.GetUsersToMatchAsync(dataUserNowConnect, cancellationToken);

            return this.Ok(ApiResponseDto.Success("yes good", users));
        }



        [HttpGet("GetUserConnected")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<QueryUser>))]
        public async Task<IActionResult> GetUserConnected(CancellationToken cancellationToken)
        {
            User dataUserNowConnect = await this._GetDataUserConnected(token_session_user, cancellationToken);

            QueryUser userDto = dataUserNowConnect.ToGetUserMapper();

            return this.Ok(ApiResponseDto.Success("yes good", userDto));
        }


        [HttpPatch("UpdateUser")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<QueryUser>))]
        public async Task<IActionResult> UpdateUser([FromForm] CommandUpdateUser commandUpdateUser, CancellationToken cancellationToken)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            User dataUserNowConnect = await this._GetDataUserConnected(token_session_user, cancellationToken);

            var newsValues = this.UpdatingCheckUser(commandUpdateUser, await _messUtils.ConvertImageToByteArrayAsync(commandUpdateUser.Profile_picture), dataUserNowConnect);

            dataUserNowConnect.Profile_picture = newsValues.Profile_picture;
            dataUserNowConnect.city = newsValues.city;
            dataUserNowConnect.sex = newsValues.sex;
            dataUserNowConnect.Description = newsValues.Description;
            dataUserNowConnect.date_of_birth = newsValues.date_of_birth;

            // TESTE TODO

            await this._repositoryU.SaveChangesAsync(cancellationToken);

            QueryUser UserDtoNewValues = newsValues.ToGetUserMapper();

            return this.Ok(ApiResponseDto.Success("yes good", UserDtoNewValues));
        }


        [HttpGet("GetUser/{Id_User}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<QueryUser>))]
        public async Task<IActionResult> GetUserId([FromRoute] int Id_User, CancellationToken cancellationToken)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            User user = await this._userRepository.GetUserByIdUserAsync(Id_User, cancellationToken);

            if (user == null) 
                return this.NotFound(ApiResponseDto.Failure("no found :/"));

            QueryUser userDto = user.ToGetUserMapper();

            return this.Ok(ApiResponseDto.Success("found", userDto));
        }




        private User UpdatingCheckUser(CommandUpdateUser setUserUpdateDto, byte[] imageData, User dataUserNowConnect)
        {
            return new User
            {
                   Profile_picture = _regexUtils.CheckPicture(setUserUpdateDto.Profile_picture)
                    ? imageData: dataUserNowConnect.Profile_picture,

                   city = _regexUtils.CheckCity(setUserUpdateDto.city) 
                   ? setUserUpdateDto.city : dataUserNowConnect.city,

                   Description = _regexUtils.CheckDescription(setUserUpdateDto.Description)
                   ? setUserUpdateDto.Description : dataUserNowConnect.Description,

                   sex = _regexUtils.CheckSex(setUserUpdateDto.sex)
                    ? setUserUpdateDto.sex : dataUserNowConnect.sex,

                   date_of_birth = _regexUtils.CheckDate(setUserUpdateDto.date_of_birth)
                   ? setUserUpdateDto.date_of_birth ?? DateTime.MinValue : dataUserNowConnect.date_of_birth,
            };
        }
    }
}