using API.Entities;
using API.Features.Users;
using API.Services;
using API.Services.Email;
using MediatR;

namespace API.Features.Authentification.Register;

internal sealed class Handler : IRequestHandler<CommandRegister , ApiResponseDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IJWTSessionUtils _jWTSessionUtils;
    private readonly IRegexUtils _regexUtils;
    private readonly IUserRepository _userRepository;
    private readonly IRepository<User> _repositoryU;
    private readonly ISendMail _sendMail;

    public Handler
    (
        IHttpContextAccessor httpContextAccessor,
        IJWTSessionUtils jWTSessionUtils,
        IRegexUtils regexUtils,
        IUserRepository userRepository,
        IRepository<User> repositoryU,
        ISendMail sendMail
    )
    {
        this._httpContextAccessor = httpContextAccessor;
        this._jWTSessionUtils = jWTSessionUtils;
        this._regexUtils = regexUtils;
        this._userRepository = userRepository;
        this._repositoryU = repositoryU;
        this._sendMail = sendMail;
    }

    public async Task<ApiResponseDto> Handle(CommandRegister commandRegister, CancellationToken cancellationToken)
    {

         var claims = this._jWTSessionUtils.GetClaimsFromCookieJWT(this._httpContextAccessor.HttpContext, this._jWTSessionUtils.NameCookieUserGoogle);

            string emailGoogle = claims?.FirstOrDefault(c => c.Type == "EmailGoogle")?.Value;
            string userIdGoogle = claims?.FirstOrDefault(c => c.Type == "userIdGoogle")?.Value;


            if (string.IsNullOrEmpty(emailGoogle) || string.IsNullOrEmpty(userIdGoogle))
            {
                return ApiResponseDto.Failure("Please login with Google before register");
            }

            var (success, message) = this._regexUtils.CheckBodyAuthRegister(commandRegister);


            if (!success)
            {
               return ApiResponseDto.Failure(message);
            }


            if (await this._userRepository.GetUserByPseudoAsync(commandRegister.Pseudo, cancellationToken))
            {
               return ApiResponseDto.Failure("Pseudo Already use");
            }


            User userGet = await this._userRepository.GetUserIdWithGoogleIdAsync(emailGoogle, userIdGoogle, cancellationToken);


            if (userGet.Id_User > 0)
            {
                await this._jWTSessionUtils.CreateSessionLoginAsync(userGet, cancellationToken);
                return ApiResponseDto.Success("User already exists, logged in", null);
            }


            var user = new User
            {
                userIdGoogle = userIdGoogle,
                EmailGoogle = emailGoogle,
                Description = commandRegister.Description,
                date_of_birth = commandRegister.Date_of_birth.HasValue ? commandRegister.Date_of_birth.Value.ToUniversalTime() : DateTime.MinValue,
                sex = commandRegister.Sex,
                Pseudo = commandRegister.Pseudo,
                city = commandRegister.City,
                account_created_at = DateTime.Now.ToUniversalTime(),
            };


            await this._repositoryU.AddAsync(user, cancellationToken);
            int? Newid_user = await this._repositoryU.SaveChangesAsync(cancellationToken);


            if (Newid_user.HasValue)
            {
                await this._jWTSessionUtils.CreateSessionLoginAsync(user, cancellationToken);
                await this._sendMail.MailRegisterAsync(emailGoogle, commandRegister.Pseudo);
                return ApiResponseDto.Success("Register finish", null);
            }

            return ApiResponseDto.Failure("Failed to create user");
    }
}