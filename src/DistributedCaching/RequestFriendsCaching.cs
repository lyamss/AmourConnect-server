using Domain.Dtos.AppLayerDtos;
using Domain.Dtos.GetDtos;
using Infrastructure.Interfaces;

namespace Infrastructure.DistributedCaching
{
    public class RequestFriendsCaching(IRequestFriendsRepository requestFriendsRepository, ICacheService CacheService) : IRequestFriendsCaching
    {
        private readonly IRequestFriendsRepository _requestFriendsRepository = requestFriendsRepository;
        private readonly ICacheService _cacheService = CacheService;

        public async Task<ICollection<GetRequestFriendsDto>> GetRequestFriendsAsync(int Id_User)
        {
            string key = Id_User.ToString() + "GetRequestFriends";

            ICollection<GetRequestFriendsDto> getRequestFriendsDtos = await _cacheService.GetAsync<ICollection<GetRequestFriendsDto>>(key);
            if(getRequestFriendsDtos is null)
            {
                ICollection<GetRequestFriendsDto> getRequestFriendsDtosR = await _requestFriendsRepository.GetRequestFriendsAsync(Id_User);

                await _cacheService.SetAsync(key, getRequestFriendsDtosR, TimeSpan.FromSeconds(30));

                return getRequestFriendsDtosR;
            }

            return getRequestFriendsDtos;
        }

        public async Task<RequestFriendForGetMessageDto> GetRequestFriendByIdAsync(int IdUserIssuer, int IdUserReceiver)
        {
            string key = IdUserIssuer.ToString() + "GetRequestFriendById" + IdUserReceiver.ToString();

            RequestFriendForGetMessageDto getRequestFriendIdCache = await _cacheService.GetAsync<RequestFriendForGetMessageDto>(key);
            if(getRequestFriendIdCache is null)
            {
                RequestFriendForGetMessageDto getRequestFriendDB = await _requestFriendsRepository.GetRequestFriendByIdAsync(IdUserIssuer, IdUserReceiver);

                await _cacheService.SetAsync(key, getRequestFriendDB, TimeSpan.FromSeconds(30));

                return getRequestFriendDB;
            }
            return getRequestFriendIdCache;
        }
    }
}