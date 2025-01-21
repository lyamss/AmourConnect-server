using Domain.Entities;
using Domain.Dtos.GetDtos;
using Domain.Dtos.AppLayerDtos;

namespace Infrastructure.Interfaces
{
    public interface IRequestFriendsRepository
    {
        Task<ICollection<GetRequestFriendsDto>> GetRequestFriendsAsync(int Id_User);
        Task<RequestFriendForGetMessageDto> GetRequestFriendByIdAsync(int IdUserIssuer, int IdUserReceiver);
        Task AddRequestFriendAsync(RequestFriends requestFriends);
        Task<RequestFriends> GetUserFriendRequestByIdAsync(int Id_User, int IdUserIssuer);
        Task UpdateStatusRequestFriendsAsync(RequestFriends friendRequest);
    }
}