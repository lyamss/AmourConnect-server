using Domain.Dtos.GetDtos;
using Domain.Entities;
using Infrastructure.Interfaces;

namespace Infrastructure.DistributedCaching
{
    public class UserCaching(IUserRepository userRepository, ICacheService CacheService) : IUserCaching
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ICacheService _cacheService = CacheService;
        public async Task<User> GetUserWithCookieAsync(string token_session_user)
        {
            string keyU = token_session_user.ToString() + "GetUserWithCookie";

            User userCache = await _cacheService.GetAsync<User>(keyU);
            if (userCache is null)
            {
                User user = await _userRepository.GetUserWithCookieAsync(token_session_user);

                await _cacheService.SetAsync(keyU, user, TimeSpan.FromSeconds(30));

                return user;
            }
            return userCache;
        }

        public async Task<ICollection<GetUserDto>> GetUsersToMatchAsync(User dataUserNowConnect)
        {
            string key = dataUserNowConnect.Id_User.ToString() + "GetUsersToMatch";

            ICollection<GetUserDto> UsersCache = await _cacheService.GetAsync<ICollection<GetUserDto>>(key);

            if (UsersCache is null)
            {
                ICollection<GetUserDto> getUserDto = await _userRepository.GetUsersToMatchAsync(dataUserNowConnect);

                await _cacheService.SetAsync(key, getUserDto, TimeSpan.FromSeconds(30));

                return getUserDto;
            }

            return UsersCache;
        }

        public async Task<User> GetUserByIdUserAsync(int Id_User)
        {
            string key = Id_User.ToString() + "GetUserByIdUser";

            User userCache = await _cacheService.GetAsync<User>(key);
            if(userCache is null)
            {
                User user = await _userRepository.GetUserByIdUserAsync(Id_User);

                await _cacheService.SetAsync(key, user, TimeSpan.FromSeconds(30));

                return user;
            }
            return userCache;
        }
    }
}