using API.Entities;
using API.Features.Users;
using API.Services;

public sealed class DataUser
{
    private readonly IUserRepository userRepository;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IJWTSessionUtils jWTSessionUtils;
    public DataUser(IUserRepository _userRepository, IJWTSessionUtils _jWTSessionUtils, IHttpContextAccessor _httpContextAccessor)
    {
        this.userRepository = _userRepository;
        this.jWTSessionUtils = _jWTSessionUtils;
        this.httpContextAccessor = _httpContextAccessor;
    }

    public async Task<User> GetDataUserConnected(CancellationToken cancellationToken) => await this.userRepository.GetUserWithCookieAsync(this.jWTSessionUtils.GetValueClaimsCookieUser(this.httpContextAccessor.HttpContext), cancellationToken);
}